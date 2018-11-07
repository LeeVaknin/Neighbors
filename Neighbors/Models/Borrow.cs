using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public User Borrower { get; set; }

        [Display(Name = "Product ID")]
        [Required(ErrorMessage = "Please provide product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


      //  [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public double Fine { get; set; }

    }
}
