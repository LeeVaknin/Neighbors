using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
	public class User: IdentityUser<int>
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string Address { get; set; }

		// Check if there is relevant attribute
		[Required]
		public string City { get; set; }

		// Collection of all the products that people borrowed from me
		[NotMapped]
		public ICollection<Borrow> BorrowedProduct { get; set; }

		[NotMapped]
		public ICollection<Product> MyProducts { get; set; }

		[NotMapped]
		// Collection of all the products that I borrowed from others
		public ICollection<Borrow> MyBorrowed { get; set; }

	}
}
