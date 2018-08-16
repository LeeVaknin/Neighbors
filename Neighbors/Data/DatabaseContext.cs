using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;

namespace Neighbors.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Neighbors.Models.User> User { get; set; }

        public DbSet<Neighbors.Models.Product> Product { get; set; }

        public DbSet<Neighbors.Models.Borrow> Borrows { get; set; }

    }
 }
