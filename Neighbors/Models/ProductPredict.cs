using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
	// The prediction of the product
	public class ProductPredict
	{
		[ColumnName("PredictedLabel")]
		public uint PredictedClusterId;

		[ColumnName("Score")]
		public float[] Distances;
	}

}
