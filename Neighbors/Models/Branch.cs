using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public float Altitude { get; set; }

        public float Longitude { get; set; }
    }
}
