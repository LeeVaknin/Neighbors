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
        public async Task AddCategory(Category newCategory)
        {
            _context.Add(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
           
        }

        public async Task UpdateCategory(int categoryId, Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Getters
        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public async Task<Category> GetCategoryById(int id)
        {

            return (await _context.Categories.FirstOrDefaultAsync(c => c.Id == id));
            
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
