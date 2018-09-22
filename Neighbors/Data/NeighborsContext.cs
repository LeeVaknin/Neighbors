using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;

namespace Neighbors.Data
{
    public class NeighborsContext : IdentityDbContext<User, Role, int>
    {
        public NeighborsContext(DbContextOptions<NeighborsContext> options)
             : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }

		public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Neighbors.Models.Branch> Branch { get; set; }


  /*      protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne<Category>(p => p.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(p => p.Id);
        }
        */
    }
    
}