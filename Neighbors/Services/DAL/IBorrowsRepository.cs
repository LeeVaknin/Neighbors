using Microsoft.AspNetCore.Mvc;
using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	public interface IBorrowsRepository
	{
        Task<int> AddBorrow(Borrow newBorrow, int productId);



        #region Getters
        //     Task<IActionResult> ProductOwner();

        Task<Borrow> GetBorrowByIdAsync(int id);

        Task<ICollection<Borrow>> GetAllBorrowsAsync();


        #endregion

        	Task<int> DeleteBorrow(int borrowId);

        //	bool UpdateBorrow(int borrowId, Borrow updatedBorrow);

        // Should add getter methods get by borrower, lander, time etc..
    }
}
