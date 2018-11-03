using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Data
{
	public class NeighborsSeeder
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
        private NeighborsContext _context;

        public NeighborsSeeder(UserManager<User> userManager, RoleManager<Role> roleManager, NeighborsContext context)
        {
			_userManager = userManager;
			_roleManager = roleManager;
            _context = context;
		}

		public async Task Seed()
		{
			await SeedRoles();
			await SeedAdminUser();
            await SeedData();
		}

		private async Task SeedRoles()
		{
			var roles = Enum.GetValues(typeof(Roles));
			foreach (var role in roles)
				if (!_roleManager.Roles.Any(r => r.Name == role.ToString()))
				{
					await _roleManager.CreateAsync(new Role { Name = role.ToString() });
				}
			}

		private	async Task SeedAdminUser()
		{
			var user = new User
			{
				UserName = "Admin",
				FirstName = "Admin",
				LastName = "Admin",
				Email = "caysebix@email.com",
				EmailConfirmed = true,
				Address="Eli-Vizel",
				City = "Rishon-Lezion",
				LockoutEnabled = false,
				SecurityStamp = Guid.NewGuid().ToString()
			};


			if (!_roleManager.Roles.Any(r => r.Name == Roles.Administrator.ToString()))
			{
				await _roleManager.CreateAsync(new Role { Name = Roles.Administrator.ToString() });
			}

			if (!_userManager.Users.Any(u => u.UserName == user.UserName))
			{
				var password = new PasswordHasher<User>();
				var hashed = password.HashPassword(user, "password");
				user.PasswordHash = hashed;
				await _userManager.CreateAsync(user);
				await _userManager.AddToRoleAsync(user, Roles.Administrator.ToString());
			}
		}
        private async Task SeedData()
        {
            dynamic data = JsonConvert.DeserializeObject(File.ReadAllText("data/data.json"));
            dynamic categories = data["Categories"];
            SeedCategories(categories);
            dynamic products = data["Products"];
            SeedProducts(products);
            dynamic branches = data["Branch"];
            SeedBranches(branches);
            dynamic users = data["Users"];
            SeedUsers(users);

        
        }
        
        private void SeedCategories(dynamic data)
        {
            for (int i = 0; i < data.Count; ++i)
            {
                Category cat = new Category { Name = data[i]["Name"], Products = { } };
                var exists = _context.Categories.Where(m => m.Name == cat.Name).Count();
                if (exists == 0)
                {
                    _context.Add(cat);
                    _context.SaveChanges();
                }
            }
        }
        
        private void SeedProducts(dynamic data)
        {
            for(int i=0; i< data.Count; i++)
            {
                DateTime from = Convert.ToDateTime(data[i]["Avilable From"]);
                DateTime until = Convert.ToDateTime(data[i]["Avilable Until"]);
                int borrowDays = Convert.ToInt32((until - from).TotalDays);
                int ownerId = data[i]["Owner Id"];
                int price = data[i]["Price"];
                string categoryName = data[i]["Category"];
                Category cat = _context.Categories.Where(m => m.Name == categoryName).First();
                Product pro = new Product { Name = data[i]["Name"], Category = cat, CategoryId=cat.Id, OwnerId = ownerId, AvailableFrom = from, AvailableUntil = until, BorrowsDays = borrowDays, Price = price };
                var exists = _context.Product.Where(m => m.Name == pro.Name && m.OwnerId == pro.OwnerId).Count();
                if (exists == 0)
                {
                    _context.Add(pro);
                    _context.SaveChanges();
                }
            }
        }

        private void SeedBranches(dynamic data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                string a = data[i]["Altitude"];
                float alt = (float)Convert.ToDouble(data[i]["Altitude"]);
                float lon = (float)Convert.ToDouble(data[i]["Longitude"]);
                Branch branch = new Branch { Address = data[i]["Address"], Altitude = alt, Longitude = lon, Description = data[i]["Description"] };
                var exists = _context.Branch.Where(m => m.Description == branch.Description).Count();
                if (exists == 0)
                {
                    _context.Add(branch);
                    _context.SaveChanges();
                }
            }
        }

        async Task SeedUsers(dynamic data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                string username = data[i]["First Name"] + data[i]["Last Name"];
                string email = username + "@email.com";
                User user = new User {Email = email, EmailConfirmed=true, UserName = username, FirstName = data[i]["First Name"], LastName = data[i]["Last Name"], Address = data[i]["Address"], City = data[i]["City"], BorrowedProduct = {}, MyProducts = { }, MyBorrowed = { } };

                if (!_userManager.Users.Any(u => u.UserName == user.UserName))
                {
                    
                    var password = new PasswordHasher<User>();
                    var hashed = password.HashPassword(user, "password");
                    user.PasswordHash = hashed;
                     await _userManager.CreateAsync(user);
                     await _userManager.AddToRoleAsync(user, Roles.Consumer.ToString());
                }
            }
        }
    }
}