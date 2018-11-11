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
        public  NeighborsContext(DbContextOptions<NeighborsContext> options)
             : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }

		public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Neighbors.Models.Branch> Branch { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(a => a.Borrow)
                .WithOne(b => b.Product)
                .HasForeignKey<Borrow>(b => b.ProductId);

            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<Borrow>()
                .HasOne(l => l.Borrower)
                .WithMany(o => o.MyBorrowed)
                .HasForeignKey(k => k.BorrowerId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            base.OnModelCreating(modelBuilder);

        }
    }
    
}