using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	public interface IProductsRepository
	{
		#region Add, Delete update
		Task<int> AddProduct(Product newProduct);

		Task<int> DeleteProduct(int productId);

		Task<int> UpdateProduct(int productId, Product product);

		#endregion

		#region Getters

		Task<Product> GetProductById(int id);

		Task<ICollection<Product>> GetProductsByNameAsync(string name);

		Task<ICollection<Product>> GetProductsByCategory(int categoryId);

		Task<ICollection<Product>> GetProductsByCity(string City);

		Task<ICollection<Product>> GetAllProducts();

		Task<ICollection<Product>> SearchForProduct(ISearchModel searchModel);

		Task<ICollection<IGrouping<Category, Product>>> GetProductsGroupedByCategory();

		Task<ICollection<Product>> GetProductsByAddress(string address);

		Task<ICollection<IGrouping<string, Product>>> GetProductsGroupedByCity();

		#endregion

		#region Helpers

		bool ProductExists(int id);

		#endregion

	}
}
