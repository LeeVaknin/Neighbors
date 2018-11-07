using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.ViewModels
{
    public class UserProductsVM
    {
        [Display(Name = "Owner name")]
        public string OwnerName { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
    }
}
