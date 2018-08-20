﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public int BorrowsDays { get; set; }

        public double Price { get; set; }


    }
}
