using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{

	public class ProductSearch: ISearchModel
	{
		public Location Location { get; set; }

		[DisplayName("Category")]
		public int CategoryId { get; set; }

		public string Name { get; set; }

		[DisplayName("Minimum Price")]
		public int MinPrice { get; set; }

		[DisplayName("Maximum Price")]
		public int MaxPrice { get; set; }

		public int Id { get; set; }
	}
}
