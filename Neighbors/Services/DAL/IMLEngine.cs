using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	public interface IMLEngine
	{
		/// <summary>
		/// Gets id of a particular product, performs prediction of the cluster it belongs to and returns the cluster label.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ClusterResult> PredictById(int id);

		Task<ICollection<Product>> GetProductsFromCluster(int id);

		Task<ClusterResult> Predict(ProductData productData, int productId);

	}
}
