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
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _categoriesRepo;

        public CategoriesController(ICategoriesRepository categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
        }

        #region Client side methods

        // GET: Categories
        public IActionResult Index(string searchStr)
        {
            return View(_categoriesRepo.GetAllCategories());
        }

		public async Task<JsonResult> ReturnJSONCategories() //It will be fired from Jquery ajax call  
		{
			var categories = await _categoriesRepo.GetAllCategories();
			return Json(categories);
		}
		public async Task<IActionResult> Search(string filter)
        {
            var res = await _categoriesRepo.GetCategoryByNameAsync(filter);
            return Json(res);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoriesRepo.GetCategoryById(id);

            return View(category ?? new Category());
        }
		public IActionResult Create()
		{
			return PartialView("_CreateCatPartial", new Category());
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(int id)
        {

            var product = await _categoriesRepo.GetCategoryById(id);
            return View();

        }

        #endregion

        public async Task<IActionResult> Details(int id)
        {

            var category = await _categoriesRepo.GetCategoryById(id);
            return View(category);
        }

        #region The actual REST methods - Server side



        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Categories")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCategory(Category category)
        {
			if (ModelState.IsValid)
			{
				await _categoriesRepo.AddCategory(category);
			}
			return PartialView("_CreateCatPartial", category);
		}


        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("/Categories/{id}")]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                await _categoriesRepo.DeleteCategory(id);
                return RedirectToAction(nameof(Index));
            }
            return View();

           
        }

        #endregion
    }

}
