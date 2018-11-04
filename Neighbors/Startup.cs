using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;
using Microsoft.AspNetCore.Identity;
using Neighbors.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Neighbors.Services;
using Neighbors.Services.DAL;
using FluentValidation;
using Neighbors.Validators;
using FluentValidation.AspNetCore;

namespace Neighbors
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        { 
            services.AddDbContext<NeighborsContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("NeighborsDb")));

			ConfigureAuthentication(services);

			services.AddSingleton<IEmailSender, EmailSender>();
			services.AddScoped<IProductsRepository, ProductsRepository>();
			services.AddScoped<ICategoriesRepository, CategoriesRepository>();
			services.AddTransient<IValidator<Category>, CategoryValidator>();
		}

		/// <summary>
		/// Added google authentication method and identity to the project.
		/// Identity will allow us to manage users by our selves, if we want.
		/// </summary>
		/// <param name="services"></param>
		private void ConfigureAuthentication(IServiceCollection services)
		{
			services.AddIdentity<User, Role>()
				.AddEntityFrameworkStores<NeighborsContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication().AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = Configuration.GetSection("Authentication").GetSection("Google")["ClientId"];
				googleOptions.ClientSecret = Configuration.GetSection("Authentication").GetSection("Google")["ClientSecret"];
			});


            services.AddMvc()
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddRazorPagesOptions(options =>
				{
					options.AllowAreas = true;
					options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
					options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
				})
               ;
            
            services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
			});

			// using Microsoft.AspNetCore.Identity.UI.Services;
		}

		private async Task SeedDB(IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var services = scope.ServiceProvider;
				var userManager = services.GetRequiredService<UserManager<User>>();
				var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var ctx = services.GetRequiredService<NeighborsContext>();
                var seeder = new NeighborsSeeder(userManager, roleManager, ctx);
				//await seeder.Seed();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
            
			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			await SeedDB(app);
		}
	}
}
