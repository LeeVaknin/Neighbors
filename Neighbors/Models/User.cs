using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required,Phone]
        public int PhoneNumber { get; set; }

        [Required,EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Address { get; set; }

        // Cheack if there is relevant attribute
        [Required]
        public string City { get; set; }

        // Add ROLE

        // Collection of all the products that people borrowed from me
        public ICollection<Borrow> BorrowedProduct { get; set; }

        public ICollection<Product> MyProducts { get; set; }

        // Collection of all the products that I borrowed from others
        public ICollection<Borrow> MyBorrowed { get; set; }


    }
}
