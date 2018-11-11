using Microsoft.EntityFrameworkCore;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
using Neighbors.Data;
using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	public class ClusterEngine : IMLEngine
	{
		private readonly NeighborsContext _context;
		static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "productsCluster.data");
		static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "productsClusterModel.zip");
		//private static readonly TextLoader _productLoader;

		public ClusterEngine(NeighborsContext context)
		{
			_context = context;
		}

		#region DAL 

		public async Task<ClusterResult> PredictById(int id)
		{
			// Check if this product was already clustered, if it was, return the result. 
			var clustered = _context.ClusterResults.FirstOrDefault(cluster => cluster.ProductId == id);
			if (clustered != null)
			{
				return clustered;
			}

			// Else, predict
			var productToCluster = _context.Product.Where(pr => pr.Id == id).Include(pr => pr.Category).FirstOrDefault();
			var prodData = new ProductData()
			{
				Price = (float)productToCluster.Price,
				BorrowDays = productToCluster.BorrowsDays,
				Category = Category.ConvertFromString(productToCluster.Category.Name)
			};

			return await Predict(prodData, productToCluster.Id);

		}

		public async Task<ICollection<Product>> GetProductsFromCluster(int id)
		{
			var result = await (from pr in _context.Product.Where(pr => pr.Borrow == null)
								join clstrToPro in (from clster in _context.ClusterResults
													where clster.ClusterId == id
													select clster.ProductId)
													on pr.Id equals clstrToPro
								select pr)
						  .Include(c => c.Category)
						  .Include(o => o.Owner)
						  .ToListAsync();
			return result;
		}

		#endregion

		#region Preprocessing and Training

		public static async Task<PredictionModel<ProductData, ProductPredict>> PreProcessMLEngine()
		{
			PredictionModel<ProductData, ProductPredict> model = Train();
			await model.WriteAsync(_modelPath);
			return model;
		}

		private static PredictionModel<ProductData, ProductPredict> Train()
		{
			var data = ExtractTrainingData();
			var pipeline = new LearningPipeline();

			pipeline.Add(new TextLoader(_dataPath).CreateFrom<ProductData>(separator: ','));
			pipeline.Add(new ColumnConcatenator(
				"Features",
				"Category",
				"Price",
				"BorrowDays"
			));

			pipeline.Add(new KMeansPlusPlusClusterer() { K = 5 });
			PredictionModel<ProductData, ProductPredict> model = null;
			try
			{
				model = pipeline.Train<ProductData, ProductPredict>();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			return model;
		}

		private static List<ProductData> ExtractTrainingData()
		{
			List<ProductData> data = new List<ProductData>();
			string line;
			using (var reader = File.OpenText(_dataPath))
			{
				while ((line = reader.ReadLine()) != null)
				{
					string convertedData = line;
					List<string> ProductFeaturesSet = convertedData.Split(',').ToList();
					ProductData bd = new ProductData
					{
						Category = int.Parse(ProductFeaturesSet[0]),
						Price = int.Parse(ProductFeaturesSet[1]),
						BorrowDays = int.Parse(ProductFeaturesSet[2]),
					};
					data.Add(bd);
				}
			}
			return data;
		}

		private static async Task<PredictionModel<ProductData, ProductPredict>> LoadModel()
		{
			PredictionModel<ProductData, ProductPredict> model;
			try
			{
				model = await PredictionModel.ReadAsync<ProductData, ProductPredict>(_modelPath);
			}
			catch
			{
				Console.WriteLine("Failed loading ML model.");
				model = null;
			}
			//if (model == null)
			//{
			//	model = await PreProcessMLEngine();
			//}
			return model;
		}

		#endregion

		#region Prediction

		public async Task<ClusterResult> Predict(ProductData productData, int productId)
		{
			var model = await LoadModel();
			var prediction = model.Predict(productData);

			// Save prediction into the database
			var result = new ClusterResult() { ProductId = productId, ClusterId = (int)prediction.PredictedClusterId };
			_context.ClusterResults.Add(result);
			await _context.SaveChangesAsync();

			return result;
		}

		#endregion

	}
}
