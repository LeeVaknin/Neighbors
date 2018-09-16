using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide product name")]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Select category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Display(Name = "Owner ID")]
        public int OwnerId { get; set; }
     //   public User User { get; set; }

        [Display(Name = "Borrows days")]
        public int BorrowsDays { get; set; }

        public double Price { get; set; }
       
    }
}
