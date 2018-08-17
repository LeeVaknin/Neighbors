using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;

namespace Neighbors.Models
{
    public class NeighborsContext : DbContext
    {
        public NeighborsContext (DbContextOptions<NeighborsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Borrow> Borrows { get; set; }

    }
 }
