using FluentValidation;
using Neighbors.Data;
using Neighbors.Models;
using Neighbors.Services.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbors.Validators
{
    public class CategoryValidator: AbstractValidator<Category>
    {
        private readonly ICategoriesRepository _categiruesRepo;

        public CategoryValidator(ICategoriesRepository catRepo)
        {
            _categiruesRepo = catRepo;
            RuleFor(x => x.Name)
                .MustAsync(BeUniqueCategoryName)
                .WithMessage("This Category already exists.");
        }

        private async Task<bool> BeUniqueCategoryName(string name, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                var dupCategory = await _categiruesRepo.GetCategoryByNameAsync(name);
                return dupCategory == null;
            }
            return false;

        }
    }
}
