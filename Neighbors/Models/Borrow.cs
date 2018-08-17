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

		[NotMapped]
		public User Lender { get; set; }

		[NotMapped]
		public User Borrower { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Fine { get; set; }


    }
}
