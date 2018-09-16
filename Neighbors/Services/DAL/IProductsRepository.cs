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
		Task AddProduct(Product newProduct);

		Task DeleteProduct(int productId);

		Task UpdateProduct(int productId, Product product);

		#endregion

		#region Getters

		Task<Product> GetProductById(int id);

		Task<ICollection<Product>> GetProductsByNameAsync(string name);

		ICollection<Product> GetProductsByCategory(Category category);

		ICollection<Product> GetProductsByCity(string City);

		ICollection<Product> GetAllProducts();

		#endregion

		#region Helpers

		bool ProductExists(int id);

        #endregion

        #region Categories 

        IEnumerable<object> GetAllCategories();

        #endregion
    }
}
