using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neighbors.Models;
using Neighbors.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using Neighbors.ViewModels;

namespace Neighbors.Services.DAL
{
	public class ProductsRepository : IProductsRepository
	{
		#region C-TOR and Data members

		private readonly NeighborsContext _context;
		private readonly SignInManager<User> _signinManager;
		private readonly ICategoriesRepository _catRepo;

		public ProductsRepository(NeighborsContext neighborsContext, ICategoriesRepository catRepo, SignInManager<User> signinManager)
		{
			_context = neighborsContext;
			_catRepo = catRepo;
			this._signinManager = signinManager;
		}

		#endregion

		#region Add, Delete and Update

		public async Task<int> AddProduct(Product newProduct)
		{
			if (newProduct.OwnerId <= 0)
			{
				var strUserId = _signinManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (Int32.TryParse(strUserId, out var userId)) newProduct.OwnerId = userId;
				else return 0;

			}
            
            newProduct.Owner = await _context.Users.FirstOrDefaultAsync(user => user.Id == newProduct.OwnerId);

            _context.Add(newProduct);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteProduct(int productId)
		{
			var product = await _context.Product.FindAsync(productId);
			if (product != null)
			{
				_context.Product.Remove(product);
			}

			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateProduct(int productId, Product product)
		{
			_context.Update(product);
			return await _context.SaveChangesAsync();
		}

        #endregion

        #region Getters

  /*      public async Task<IActionResult> ProductOwner()
        {

            var q = from p in _context.Product
                    join u in _context.Users on p.Owner.Id equals u.Id
                    select new UserProductsVM()
                    {
                        ProductName = p.Name,
                        OwnerName = u.FirstName + ' ' + u.LastName
                    };

            return View(await q.ToListAsync());
        }
        */
        public async Task<ICollection<Product>> GetAllProducts()
		{
			return await _context.Product.Include(c => c.Category).ToListAsync();
		}

		public async Task<Product> GetProductById(int id)
		{
			return (await _context.Product.Include(c => c.Category).FirstOrDefaultAsync(pr => pr.Id == id));
		}

		public async Task<ICollection<Product>> GetProductsByCategory(Category category)
		{
			var response = await (from pr in _context.Product
								  where pr.Category == category
								  select pr).Include(c => c.Category).ToListAsync();
			return response;
		}

		public async Task<ICollection<Product>> GetProductsByCity(string city)
		{
			var response = await (from pr in _context.Product
								  join cityUsr in
									  (from usr in _context.Users where usr.City == city select usr.Id)
								  on pr.OwnerId equals cityUsr
								  select pr).Include(c => c.Category).ToListAsync();
			return response;
		}

		public async Task<ICollection<Product>> GetProductsByNameAsync(string name)
		{
			var response = await _context.Product.Where(pr => pr.Name.Contains(name)).Include(c => c.Category).ToListAsync();
			return response;
		}

		#endregion

		#region Helper

		public bool ProductExists(int id)
		{
			return _context.Product.Any(e => e.Id == id);
		}

		public Task<ICollection<Product>> GetProducts(ProductSearch searchModel)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
