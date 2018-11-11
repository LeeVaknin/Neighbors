using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;
using Neighbors.Services.DAL;

namespace Neighbors.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductsRepository _productsRepo;



		public ProductsController(IProductsRepository productsRepository)
		{
			_productsRepo = productsRepository;
		}

		#region Client side getters

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

	
		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var product = await _productsRepo.GetProductById(id);
            if (null == product)
            {
                return RedirectToAction("InvalidAction", "Error");
            }
            return View(product);
		}

		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var product = await _productsRepo.GetProductById(id);
            if (null == product || (User.Identity.Name != product.Owner.UserName))
            {
                return RedirectToAction("InvalidAction", "Error");
            }
            
            return View(product);
		}

		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var product = await _productsRepo.GetProductById(id);
            if (null == product || (User.Identity.Name != product.Owner.UserName))
            {
                return RedirectToAction("InvalidAction", "Error");
            }
            return View(product);
		}

		#endregion

		#region Server side methods

		[AllowAnonymous]
		public async Task<IActionResult> GetJsonProducts()
		{
			var result = await _productsRepo.GetAllProducts();
			return PartialView("/Views/Products/_ProductItem.cshtml",result);
		}


		[AllowAnonymous]
		[HttpPost("/Products/Search")]
		public async Task<IActionResult> Search([FromBody] ProductSearch filter)
		{
			var res = await _productsRepo.SearchForProduct(filter);
			return PartialView("/Views/Products/_ProductItem.cshtml", res);
		}

		[AllowAnonymous]
		[HttpGet("/Products/GroupByCity")]
		public async Task<IActionResult> GroupByCities()
		{
			var res = await _productsRepo.GetProductsGroupedByCity();
			return Json(res);
		}

		[AllowAnonymous]
		[HttpGet("/Products/GroupByCategory")]
		public async Task<IActionResult> GroupByCategories()
		{
			var res = await _productsRepo.GetProductsGroupedByCategory();
			return Json(res);
		}


		// POST: Products/Create
		[HttpPost("/Products")]
		[Authorize]
		public async Task<IActionResult> AddNewProduct(Product product)
		{
			if (ModelState.IsValid)
			{

				await _productsRepo.AddProduct(product);
				return RedirectToAction(nameof(Index));
			}

			return PartialView("_CreateProductPartial", product);
		}

		// POST: Products/Edit/5
		[HttpPost("/Products/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize]
		public async Task<IActionResult> UpdateProduct(int id, Product product)
		{
			if (ModelState.IsValid)
			{
				if (id != product.Id || !_productsRepo.ProductExists(id))
				{
					return NotFound();
				} 
				await _productsRepo.UpdateProduct(id, product);
                return RedirectToAction("Index", "Identity/Account/Manage");
            }
            return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost("/Products/Delete/{id}")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				if(await _productsRepo.DeleteProduct(id) == -1)
                {
                    return RedirectToAction("InvalidAction", "Error");
                }
				return RedirectToAction("Index", "Identity/Account/Manage");
            }
			return View();
		}

		#endregion
	}
}
