using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Data
{
	public class UsersContext : IdentityDbContext<User, Role, int>
	{
		public UsersContext(DbContextOptions<UsersContext> options)
			: base(options)
		{
		}

	}
}
