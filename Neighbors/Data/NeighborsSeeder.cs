using Microsoft.AspNetCore.Identity;
using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Data
{
	public class NeighborsSeeder
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public NeighborsSeeder(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task Seed()
		{
			await SeedRoles();
			await SeedAdminUser();
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
	}
}