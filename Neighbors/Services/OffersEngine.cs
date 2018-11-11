using Microsoft.AspNetCore.Identity;
using Neighbors.Models;
using Neighbors.Services.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neighbors.Services
{
	public class OffersEngine
	{
		private readonly IMLEngine _bl;
		private readonly SignInManager<User> _signInManager;

		public OffersEngine(IMLEngine mlBL, SignInManager<User> signInManager)
		{
			_bl = mlBL;
			_signInManager = signInManager;
		}

		public async Task<ICollection<Product>> OfferProductsForUser()
		{
			var result = new List<Product>();

			// Get logged in user
			var loggedInUser = _signInManager.Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier); // pull logged in user's id
			var user = await _signInManager.UserManager.FindByIdAsync(loggedInUser);
			if (string.IsNullOrEmpty(loggedInUser)) { return result; }

			// Get clusters
			var clusters = await GetRelevantClusters(user.MyBorrowed);

			// GetProducts from those clusters
			result.AddRange(await GetRelevantProducts(clusters));

			// Sort the result to display the closest item for the user
			SortMe(result, user.City);

			return result;

		}

		#region Private Methods

		private async Task<ICollection<Product>> GetRelevantProducts(ICollection<ClusterResult> clusters)
		{
			var relevantProducts = new List<Product>();
			if (clusters == null) { return relevantProducts; }
			foreach (var cluster in clusters)
			{
				var products = await _bl.GetProductsFromCluster(cluster.ClusterId);
				relevantProducts.AddRange(products);
			}
			return relevantProducts.Distinct().ToArray();
		}

		private async Task<ICollection<ClusterResult>> GetRelevantClusters(ICollection<Borrow> favorites)
		{
			var relevantClusters = new List<ClusterResult>();
			if (favorites == null) { return relevantClusters; }
			foreach (var borrow in favorites)
			{
				var productId = borrow.ProductId;
				var cluster = await _bl.PredictById(productId);
				relevantClusters.Add(cluster);
			}
			return relevantClusters.Distinct().ToArray();
		}

		private void SortMe(IList<Product> products, string userCity)
		{
			var temp = products.First();

			// set list pointers
			int i = 0;
			int j = 0;

			// iterate and swap until all sorted by given city
			while (j < products.Count)
			{
				if (products[i].Owner?.City != userCity && products[j].Owner?.City == userCity)
				{
					// swap
					var tmp = products[i];
					products[i] = products[j];
					products[j] = tmp;
					++i;
				}
				++j;
			}

		}

		#endregion
	}
}
