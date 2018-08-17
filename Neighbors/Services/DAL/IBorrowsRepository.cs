using Neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbors.Services.DAL
{
	interface IBorrowsRepository
	{
		bool AddBorrow(Borrow newBorrow);

		bool DeleteBorrow(int borrowId);

		bool UpdateBorrow(int borrowId, Borrow updatedBorrow);

		// Should add getter methods get by borrower, lander, time etc..
	}
}
