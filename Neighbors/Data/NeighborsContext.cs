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
           
            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<Borrow>()
                .HasOne(l => l.Lender)
                .WithMany(o => o.MyBorrowed)
                .HasForeignKey(k => k.LenderId);
                

        /*    modelBuilder.Entity<Borrow>()
                .HasOne(o => o.Borrower)
                .WithMany(i => i.BorrowedProductFromMe)
                .HasForeignKey(k => k.BorrowerId);
                //.OnDelete(DeleteBehavior.Cascade);
*/
            base.OnModelCreating(modelBuilder);
            //.HasOne<Category>(p => p.Category)
            //.WithMany(c => c.CategoryProducts)
            //.HasForeignKey(p => p.Id);
        }
    }
    
}