using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbors.Areas.Identity.Pages.Account.Manage
{
	public class ApplicationUserStore : UserStore<User, Role, NeighborsContext, int>
	{
		public ApplicationUserStore(NeighborsContext context)
	   : base(context)
		{
		}

		public override async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
		{
			return await Users.Include(b => b.MyBorrowed)
				.ThenInclude(p => p.Product)
				.ThenInclude(o => o.Owner)
				.Include(p => p.MyProducts)
				.ThenInclude(c => c.Category)
				.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
		}

		public override async Task<User> FindByEmailAsync(string userEmail, CancellationToken cancellationToken = new CancellationToken())
		{
			return await Users.Include(b => b.MyBorrowed)
				.ThenInclude(p => p.Product)
				.ThenInclude(o => o.Owner)
				.Include(p => p.MyProducts)
				.ThenInclude(c => c.Category)
				.FirstOrDefaultAsync(u => u.Email.ToString() == userEmail);
		}
	}
}
