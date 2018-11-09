using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;

namespace Neighbors.Services.DAL
{
    public class CategoriesRepository : ICategoriesRepository
    {
        #region C-TOR and Data members

        private readonly NeighborsContext _context;

        public CategoriesRepository(NeighborsContext neighborsContext)
        {
            _context = neighborsContext;
        }

        #endregion

        #region Add, Delete update
        public async Task<int> AddCategory(Category newCategory)
        {
            _context.Add(newCategory);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            return await _context.SaveChangesAsync();
           
        }

        public async Task<int> UpdateCategory(int categoryId, Category category)
        {
            _context.Update(category);
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Getters
        public async Task<ICollection<Category>> GetAllCategories()
        {
            return await _context.Categories.Include(p => p.Products).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return (await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == id)); 
        }

        public async Task<ICollection<Category>> GetCategoryByNameAsync(string name)
        {
            var response = await _context.Categories.Where(c => c.Name.Contains(name)).ToListAsync();
            return response;
        }

        #endregion

        #region Helpers
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
        #endregion

    }
}
