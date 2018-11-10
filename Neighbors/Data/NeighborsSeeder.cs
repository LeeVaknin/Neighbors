﻿using Microsoft.AspNetCore.Identity;
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

        private async Task SeedAdminUser()
        {
            var user = new User
            {
                UserName = "makaka@email.com",
                FirstName = "Mrs.",
                LastName = "Administrator",
                Email = "makaka@email.com",
                EmailConfirmed = true,
                Address = "Eli-Vizel",
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
            await SeedProducts(products);
            dynamic branches = data["Branch"];
            SeedBranches(branches);
            dynamic users = data["Users"];
            SeedUsers(users);
            dynamic borrow = data["Borrow"];
            SeedBorrow(borrow);
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

        private async Task SeedProducts(dynamic data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                try
                {
                    DateTime from = Convert.ToDateTime(data[i]["Avilable From"]);
                    DateTime until = Convert.ToDateTime(data[i]["Avilable Until"]);
                    int borrowDays = Convert.ToInt32((until - from).TotalDays);
                    int ownerId = data[i]["Owner Id"];
                    int price = data[i]["Price"];
                    string categoryName = data[i]["Category"];
                    if (await _userManager.FindByIdAsync(ownerId.ToString()) == null)
                    {
                        ownerId = _userManager.Users.LastOrDefault().Id;
                    }
                    Category cat = _context.Categories.Where(m => m.Name == categoryName).First();
                    Product pro = new Product { Name = data[i]["Name"], Category = cat, CategoryId = cat.Id, OwnerId = ownerId, AvailableFrom = from, AvailableUntil = until, BorrowsDays = borrowDays, Price = price };
                    var exists = _context.Product.Where(m => m.Name == pro.Name && m.OwnerId == pro.OwnerId).Count();
                    if (exists == 0)
                    {
                        _context.Add(pro);
                        _context.SaveChanges();
                    }
                }
                catch(Exception e)
                { }
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

        async Task SeedBorrow(dynamic data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                try
                {
                    DateTime startDate = Convert.ToDateTime(data[i]["StartDate"]);
                    DateTime endDate = Convert.ToDateTime(data[i]["EndDate"]);
                    int price = data[i]["Fine"];
                    var users = _context.Users.ToArray();
                    var rand = new Random();
                    User user = users[rand.Next(users.Count())];
                    var products = _context.Product.Where(p => p.OwnerId != user.Id && p.AvailableFrom > startDate).ToArray();
                    if (products.Count() > 0)
                    {
                        Product product = products[rand.Next(products.Count())];
                        Borrow borrow = new Borrow
                        {
                            StartDate = startDate,
                            EndDate = endDate,
                            Fine = product.Price,
                            Borrower = user,
                            BorrowerId = user.Id,
                            Product = product,
                            ProductId = product.Id
                        };
                        var exists = _context.Borrows.Where(b => b.StartDate == borrow.StartDate && b.Id == borrow.Id && b.ProductId == borrow.ProductId).Count();
                        if (exists == 0)
                        {
                            _context.Add(borrow);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        --i;
                    }
                    
                }
                catch(Exception e)
                {
                    int a = 5;
                }
                
            }
        }

        async Task SeedUsers(dynamic data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (!_roleManager.Roles.Any(r => r.Name == Roles.Consumer.ToString()))
                {
                    await _roleManager.CreateAsync(new Role { Name = Roles.Consumer.ToString() });
                }

                string username = data[i]["First Name"] + data[i]["Last Name"];
                string email = username + "@email.com";

                User user = new User { Email = email, EmailConfirmed = true, LockoutEnabled = false, SecurityStamp = Guid.NewGuid().ToString(), UserName = email, FirstName = data[i]["First Name"], LastName = data[i]["Last Name"], Address = data[i]["Address"], City = data[i]["City"], TwoFactorEnabled = false };
                if (!_userManager.Users.Any(u => u.UserName == user.UserName))
                {

                    var password = new PasswordHasher<User>();
                    var hashed = password.HashPassword(user, "password");
                    user.PasswordHash = hashed;
                    try
                    {
                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, Roles.Consumer.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}