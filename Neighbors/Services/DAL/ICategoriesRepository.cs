using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
    public interface ICategoriesRepository
    {
        #region Add, Delete update
        Task<int> AddCategory(Category newCategory);

        Task<int> DeleteCategory(int categoryId);

        Task<int> UpdateCategory(int categoryId, Category category);

        #endregion

        #region Getters

        Task<Category> GetCategoryById(int id);

        Task<ICollection<Category>> GetCategoryByNameAsync(string name);

        Task<ICollection<Category>> GetAllCategories();

        #endregion

        #region Helpers

        bool CategoryExists(int id);

        #endregion

   


    }
}
