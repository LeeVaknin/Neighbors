using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	interface IProductsRepository
	{
		bool AddProduct(Product newProduct);

		bool DeleteProduct(int productId);

		bool UpdateProduct(int productId, Product updatedProduct);

		Product GetProductByName(int id);

		ICollection<Product> GetProductsByName(string name);

		ICollection<Product> GetProductsByCategory(Category category);

		ICollection<Product> GetProductsByCity(string City);
	}
}
