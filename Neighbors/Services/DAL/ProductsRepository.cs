﻿using System.Collections.Generic;
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
			newProduct.BorrowsDays = (newProduct.AvailableUntil - newProduct.AvailableUntil).Days;
            
         //   newProduct.Owner = await _context.Users.FirstOrDefaultAsync(user => user.Id == newProduct.OwnerId);

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

        public async Task<ICollection<Product>> GetAllProducts()
		{
            var strUserId = _signinManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await (from pr in _context.Product
                                  where pr.Borrow == null
                                  select pr).Include(c => c.Category)
                                  .Include(o => o.Owner)
                                  .ToListAsync();
            return response;
		}

		public async Task<ICollection<CountModel>> GetProductsGroupedByCategory() {
			var result = await _context.Product.GroupBy(product => product.Category)
				.Select(group => new CountModel() { Name = group.Key.Name, Count = group.Count() })
				.ToListAsync();
			return result;
		}

		public async Task<ICollection<CountModel>> GetProductsGroupedByCity()
		{
			var result = await (from product in _context.Product
						  join cityUsr in _context.Users
						  on product.OwnerId equals cityUsr.Id
						  group product by cityUsr.City into groupByCity
						  select new CountModel() {  Name = groupByCity.Key,  Count = groupByCity.Count() }).ToListAsync();
			return result;
		}

		public async Task<Product> GetProductById(int id)
		{
			return (await _context.Product
                .Include(c => c.Category)
            //  .Include(b => b.Borrow)
                .Include(u => u.Owner)
                .FirstOrDefaultAsync(pr => pr.Id == id));
		}

		public async Task<ICollection<Product>> GetProductsByAddress(string address)
		{
			var response = await (from pr in _context.Product
								  join cityUsr in
									  (from usr in _context.Users where (usr.Address.Contains(address) || address.Contains(usr.Address))select usr.Id)
												 on pr.OwnerId equals cityUsr
												 select pr).ToListAsync();
			return response;
		}

		public async Task<ICollection<Product>> GetProductsByCategory(int categoryId)
		{
			var response = await (from pr in _context.Product
								  where pr.Category.Id == categoryId
								  select pr)
                                  .Include(c => c.Category)
                              //  .Include(b => b.Borrow)
                                  .Include(u => u.Owner)
                                  .ToListAsync();
			return response;
		}

		public async Task<ICollection<Product>> GetProductsByCity(string city)
		{
			var response = await (from pr in _context.Product
								  join cityUsr in
									  (from usr in _context.Users where usr.City == city select usr.Id)
								  on pr.OwnerId equals cityUsr
								  select pr)
                                  .Include(c => c.Category)
                                  //  .Include(b => b.Borrow)
                                  .Include(u => u.Owner)
                                  .ToListAsync();
			return response;
		} 

		public async Task<ICollection<Product>> GetProductsByNameAsync(string name)
		{
			var response = await _context.Product.Where(pr => (pr.Name.Contains(name) || name.Contains(pr.Name)))
                .Include(c => c.Category)
                //  .Include(b => b.Borrow)
                .Include(u => u.Owner)
                .ToListAsync();
			return response;
		}

        #endregion

        #region Search

        public async Task<ICollection<Product>> SearchForProduct(ISearchModel searchModel)
		{
			var productSearch = searchModel as ProductSearch;

			if (productSearch == null) { return null; }

			// join products and cities into one db
			var joinedDb = (from pr in _context.Product
							join cityUsr in _context.Users
							on pr.OwnerId equals cityUsr.Id
							select new { product = pr, city = cityUsr.City, address = cityUsr.Address, owner = cityUsr.FullName});

			// Check if the name was filled, if not, add the result to the list
			try
			{
				var result = joinedDb.Where(productJoin => productSearch.Name == "" ||
											productSearch.Name == null ||
											productJoin.product.Name.Contains(searchModel.Name) ||
											searchModel.Name.Contains(productJoin.product.Name))
						//// Check if the category was chosen, if not, add the result to the list
						.Where(productJoin => productSearch.CategoryId == 0 ||
										productSearch.CategoryId == productJoin.product.CategoryId)
						// Check if the city was filled, if not, add the result to the list
						.Where(productJoin => productSearch.Location.City == "" ||
											productSearch.Location.City == null ||
											productJoin.city == productSearch.Location.City)
						// Check if the street was filled, if not, add the result to the list
						.Where(productJoin => productSearch.Location.StreetAddress == "" ||
											productSearch.Location.StreetAddress == null ||
											productSearch.Location.StreetAddress.Contains(productJoin.address) ||
											productJoin.address.Contains(productSearch.Location.StreetAddress))

						.Select(productJoin => productJoin.product)
						.Include(pr => pr.Category)
						.Include(pr => pr.Owner);
				return await (result.Distinct()).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new List<Product>();
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
