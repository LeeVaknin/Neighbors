using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide category name")]
        [Display(Name = "Category name")]
        public string Name{ get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
