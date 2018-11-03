using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
	public class ProductSearch: SearchModel
	{
		public Location Location { get; set; }

		public string Category { get; set; }

	}
}
