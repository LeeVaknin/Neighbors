using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		#region View Getters

		// GET: Products
		public IActionResult Index(string SearchString)
		{
			return View(_productsRepo.GetAllProducts());
		}

		// GET: Products
		public async Task<IActionResult> Search(string filter)
		{
			var res = await _productsRepo.GetProductsByNameAsync(filter);
			return Json(res);
		}

		[HttpGet("/Products/Details/{id}")]
		public async Task<IActionResult> Details(int id)
		{
			var product = await _productsRepo.GetProductById(id);
			return View(product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
            var response = _productsRepo.GetAllCategories();

            ViewBag.CategoryId = new SelectList(response, "Value", "Text");
            return View();
		}

		[HttpGet("/Products/Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var product = await _productsRepo.GetProductById(id);
			return View();
		}

		[HttpGet("/Products/Edit/{id}")]
		public async Task<IActionResult> Edit(int id)
		{
			
			var product = await _productsRepo.GetProductById(id);
			return View(product);
		}

		#endregion

		#region Add, Delete, Update
		// POST: Products/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddNewProduct(Product product)
		{
			if (ModelState.IsValid)
			{
				await _productsRepo.AddProduct(product);
				return RedirectToAction(nameof(Index));
			}

			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost("/Products/Edit/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Product product)
		{
			if (ModelState.IsValid)
			{
				if (id != product.Id || !_productsRepo.ProductExists(id))
				{
					return NotFound();
				}

				await _productsRepo.UpdateProduct(id, product);
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost("/Products/Delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				await _productsRepo.DeleteProduct(id);
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		#endregion
	}
}
