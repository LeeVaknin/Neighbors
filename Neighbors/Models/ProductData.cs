﻿using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
	// The products that the user already has
	public class ProductData
	{
		[Column("0")]
		public float Category;

		[Column("1")]
		public float Price;

		[Column("2")]
		public float BorrowDays;
	}
}
