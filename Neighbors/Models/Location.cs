using Newtonsoft.Json;
using System.ComponentModel;

namespace Neighbors.Models
{
	public class Location
	{
		[JsonProperty(PropertyName = "city")]
		[DisplayName("City")]
		public string City { get; set; }

		[DisplayName("StreetAddress")]
		[JsonProperty(PropertyName = "streetAddress")]
		public string StreetAddress{ get; set; }

		[DisplayName("PostalCode")]
		[JsonProperty(PropertyName = "postalCode")]
		public int PostalCode { get; set; }

	}
}