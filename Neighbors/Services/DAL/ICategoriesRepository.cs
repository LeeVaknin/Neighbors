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
        Task AddCategory(Category newCategory);

        Task DeleteCategory(int categoryId);

        Task UpdateCategory(int categoryId, Category category);

        #endregion

        #region Getters

        Task<Category> GetCategoryById(int id);

        Task<ICollection<Category>> GetCategoryByNameAsync(string name);
        ICollection<Category> GetAllCategories();

        #endregion

        #region Helpers

        bool CategoryExists(int id);

        #endregion

        //   IEnumerable<object> GetAllCategories();


    }
}
