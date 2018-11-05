using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{

	public class ProductSearch: SearchModel
	{
		public Location Location { get; set; }

		[DisplayName("Category")]
		public string CategoryId { get; set; }


	}
}
