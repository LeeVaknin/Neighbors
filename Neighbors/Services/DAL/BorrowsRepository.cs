using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;

namespace Neighbors.Services.DAL
{
    public class BorrowsRepository : IBorrowsRepository
    {
        #region C-TOR and Data members

        private readonly NeighborsContext _context;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signinManager;
        private readonly IProductsRepository _proRepo;

        public BorrowsRepository(NeighborsContext neighborsContext, IProductsRepository proRepo, SignInManager<User> signinManager, UserManager<User> userManager)
        {
            _context = neighborsContext;
			_userManager = userManager;
            _proRepo = proRepo;
            _signinManager = signinManager;
        }

        #endregion

        #region Add, Delete and Update
        public async Task<int> AddBorrow(Borrow newBorrow, int productId)
        {
            var strUserId = _signinManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var product = await _context.Product.FirstOrDefaultAsync(pro => pro.Id == productId);
			// Check if the user is the owner of this products
			var user = await _userManager.FindByIdAsync(strUserId);
			if (user.MyProducts.Contains(product)) { return 0; }

            newBorrow.ProductId = productId;
            newBorrow.BorrowerId = product.OwnerId;
            
            if (Int32.TryParse(strUserId, out var userId))
                newBorrow.BorrowerId = userId;
            else return 0;
    
            newBorrow.StartDate = product.AvailableFrom;
            newBorrow.EndDate = product.AvailableUntil;
            newBorrow.Fine = product.Price;

            _context.Add(newBorrow);
            return await _context.SaveChangesAsync();
        }
       
        public async Task<int> DeleteBorrow(int borrowId)
        {
			// Check if the user is the owner of this products
			var strUserId = _signinManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(strUserId);
			var borrow = await _context.Borrows.FindAsync(borrowId);

			if (user.MyBorrowed.Contains(borrow)) { return 0; }

            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
            }

            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Getters
        public async Task<ICollection<Borrow>> GetAllBorrowsAsync()
        {
            return await _context.Borrows.Include(b => b.Borrower)
                .Include(b => b.Product)
                .ToListAsync();
        }

        public async Task<Borrow> GetBorrowByIdAsync(int id)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Borrower)
                .Include(b => b.Product)
                .ThenInclude(o => o.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            return borrow;
        }
    }
    #endregion


}
