using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
	public class ClusterResult
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int ClusterId { get; set; }
		[Required]
		public int ProductId { get; set; }

	}
}
