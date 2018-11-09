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
	[Authorize]
	public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _categoriesRepo;

        public CategoriesController(ICategoriesRepository categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
        }

		#region Client side methods
		[AllowAnonymous]
		public async Task<IActionResult> Index(string searchStr)
        {
            return View(await _categoriesRepo.GetAllCategories());
        }

		[AllowAnonymous]
		public async Task<IActionResult> Search(string filter)
        {
            var res = await _categoriesRepo.GetCategoryByNameAsync(filter);
            return Json(res);
        }

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoriesRepo.GetCategoryById(id);

            return View(category ?? new Category());
        }

		[Authorize(Roles = "Administrator")]
		public IActionResult Create()
		{
			return PartialView("_CreateCatPartial", new Category());
		}

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Delete(int id)
        {
			var category = await _categoriesRepo.GetCategoryById(id);
			return View();
		}

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Details(int id)
        {

            var category = await _categoriesRepo.GetCategoryById(id);
            if(category == null)
            {
                return NotFound();
            } 
            return View(category);
        }

		#endregion

		#region The actual REST methods - Server side

		[AllowAnonymous]
		[HttpGet("/Categories")]
		public async Task<IActionResult> GetCategories() //It will be fired from Jquery ajax call  
		{
			var categories = await _categoriesRepo.GetAllCategories();
			return Json(categories);
		}

		[HttpPost("/Categories")]
		public async Task<IActionResult> AddNewCategory([FromBody] Category category)
        {
			if (ModelState.IsValid)
			{
				var res = await _categoriesRepo.AddCategory(category);
				if (res > 0)
				{
					category = (await _categoriesRepo.GetCategoryByNameAsync(category.Name)).FirstOrDefault();
					if (category != null)
					{
						return Json(new { model = category, isValid = true, err = "" });
					}
				}
			}
			// bad practice, but I'm lazy.
			return Json(new { model = category,
				isValid = false,
				error = $"Couldn't create category with the name: {category.Name}" });
		}


        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("/Categories/{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> UpdateCategory(int id, Category category)
        {

            if (ModelState.IsValid)
            {
                if (id != category.Id || !_categoriesRepo.CategoryExists(id))
                {
                    return NotFound();
                }

                await _categoriesRepo.UpdateCategory(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }

        // POST: Categories/Delete/5
        [HttpDelete("/Categories/{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                await _categoriesRepo.DeleteCategory(id);
                return RedirectToAction("Index", "Identity/Account/Manage");
            }
            return View();

           
        }

        #endregion
    }

}
