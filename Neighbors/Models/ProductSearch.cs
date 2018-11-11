using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{

	public class ProductSearch: ISearchModel
	{
		[JsonProperty(PropertyName = "location")]
		public Location Location { get; set; }

		[DisplayName("Category")]
		[JsonProperty(PropertyName = "categoryId")]
		public int CategoryId { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DisplayName("Name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "minPrice")]
		[DisplayName("Minimum Price")]
		public int MinPrice { get; set; }

		[JsonProperty(PropertyName = "maxPrice")]
		[DisplayName("Maximum Price")]
		public int MaxPrice { get; set; }

		public int Id { get; set; }
	}
}
