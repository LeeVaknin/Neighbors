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
            modelBuilder.Entity<Branch>().HasData(new Branch { Id = 1, Address = "Eli Vizel 2, Rishon Lezion" , Altitude = 31.9689922F, Description = "Main headquarters" , Longitude = 34.77067F });
            modelBuilder.Entity<Branch>().HasData(new Branch { Id = 2, Address = "Azrieli Center" , Altitude = 32.0732231F, Description = "RnD Center", Longitude = 34.79225F});
            modelBuilder.Entity<Branch>().HasData(new Branch { Id = 3, Address = "Rupin rd 15, Haifa", Altitude = 32.79269F, Description = "Support center", Longitude = 35.0008278F});


            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Name)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
            //.HasOne<Category>(p => p.Category)
            //.WithMany(c => c.CategoryProducts)
            //.HasForeignKey(p => p.Id);
        }
    }
    
}