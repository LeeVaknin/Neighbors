using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Neighbors.Models
{

	public enum CategoryTypes
	{

	}

	public class Category
	{
		private static readonly Dictionary<string, int> _mappedCat = new Dictionary<string, int> {
			{ "ConstructionTools", 1},
			{ "GardenTools", 2},
			{ "Gaming", 3 },
			{ "HairDressing", 4},
			{ "CleaningTools", 5},
			{ "WomenClothes" ,6},
			{ "MenClothes" ,7 },
			{ "Toys" ,8},
			{ "Kitchen" ,9},
			{ "Furniture", 10},
			{ "Beauty", 11},
			{ "TravelEquipment", 12},
			{ "Bags", 13},
			{ "Others" , 14}
		};

		public int Id { get; set; }

		[Required(ErrorMessage = "Please provide category name")]
		[Display(Name = "Category name")]
		public string Name { get; set; }

		public virtual IList<Product> Products { get; set; }

		public static int ConvertFromString(string catName)
		{
			var trrimed = catName.Replace(" ", string.Empty);
			return _mappedCat.TryGetValue(trrimed, out var id) ? id : _mappedCat["Others"];
		}

		public static string ConvertFromString(int catId)
		{
			var result = _mappedCat.Values.ToList().IndexOf(catId);
			return result >= 0 ? _mappedCat.Keys.ToArray()[result] : "Others";
		}

		public static int AddCategory(string catName)
		{
			if (_mappedCat.TryGetValue(catName, out int id)) { return id; }
			var length = _mappedCat.Keys.Count + 1;
			_mappedCat.Add(catName, length);
			return length;

		}
	}
}
