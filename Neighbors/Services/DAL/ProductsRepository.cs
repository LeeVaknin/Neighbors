using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neighbors.Models;
using Neighbors.Data;
using Microsoft.EntityFrameworkCore;

namespace Neighbors.Services.DAL
{
	public class ProductsRepository : IProductsRepository
	{
		#region C-TOR and Data members

		private readonly NeighborsContext _context;

		public ProductsRepository(NeighborsContext neighborsContext)
		{
			_context = neighborsContext;
		}

		#endregion

		#region Add, Delete and Update

		public async Task AddProduct(Product newProduct)
		{
			_context.Add(newProduct);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProduct(int productId)
		{
			var product = await _context.Product.FindAsync(productId);
			if (product != null)
			{
			_context.Product.Remove(product);
			}

			await _context.SaveChangesAsync();
		}

		public async Task UpdateProduct(int productId, Product product)
		{
			_context.Update(product);
			await _context.SaveChangesAsync();

		}

		#endregion

		#region Getters

		public ICollection<Product> GetAllProducts()
		{
			return _context.Product.ToList();
		}

		public async Task<Product> GetProductById(int id)
		{
			return (await _context.Product.FirstOrDefaultAsync(pr => pr.Id == id));
		}

		public ICollection<Product> GetProductsByCategory(Category category)
		{
			throw new NotImplementedException();
		}

		public ICollection<Product> GetProductsByCity(string City)
		{
			throw new NotImplementedException();
		}

		public async Task<ICollection<Product>> GetProductsByNameAsync(string name)
		{
			var response = await _context.Product.Where(pr => pr.Name.Contains(name)).ToListAsync();
			return response;
		}

		#endregion

		#region Helper

		public bool ProductExists(int id)
		{
			return _context.Product.Any(e => e.Id == id);
		}

		#endregion
	}
}
