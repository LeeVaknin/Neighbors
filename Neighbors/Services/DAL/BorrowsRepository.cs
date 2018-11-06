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
        private readonly SignInManager<User> _signinManager;
        private readonly IProductsRepository _proRepo;

        public BorrowsRepository(NeighborsContext neighborsContext, IProductsRepository proRepo, SignInManager<User> signinManager)
        {
            _context = neighborsContext;
            _proRepo = proRepo;
            this._signinManager = signinManager;
        }

        #endregion

        #region Add, Delete and Update
        public async Task<int> AddBorrow(Borrow newBorrow, int productId)
        {
          //  var strUserId = _signinManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.Product.FirstOrDefaultAsync(pro => pro.Id == productId);
                   
            //newBorrow.Product = product;
            newBorrow.ProductId = productId;

            //newBorrow.Lender = product.Owner;
            //newBorrow.Lender = product.Owner;
            newBorrow.LenderId = product.OwnerId;

            //newBorrow.Borrower = await _context.Users.FirstOrDefaultAsync(user => user.Id.ToString() == strUserId);
        /*    if (Int32.TryParse(strUserId, out var userId)) newBorrow.BorrowerId = userId;
            else return 0;
*/
            _context.Add(newBorrow);
            return await _context.SaveChangesAsync();
        }
       
        public async Task<int> DeleteBorrow(int borrowId)
        {
            var borrow = await _context.Borrows.FindAsync(borrowId);
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
            return await _context.Borrows.Include(b => b.Lender).Include(b => b.Product).ToListAsync();
        }

        public async Task<Borrow> GetBorrowByIdAsync(int id)
        {
            var borrow = await _context.Borrows
                //.Include(b => b.Borrower)
                .Include(b => b.Lender)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            return borrow;
        }
    }
    #endregion


}
