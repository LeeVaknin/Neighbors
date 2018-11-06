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
           // return await Users.Include(p => p.MyProducts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return await Users.Include(p => p.MyProducts).Include(b => b.MyBorrowed).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        }

    }
}
