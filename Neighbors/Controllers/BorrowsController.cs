using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;
using Neighbors.Services.DAL;

namespace Neighbors.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly IBorrowsRepository _borrowRepo;

        public BorrowsController(IBorrowsRepository borrowRepository)
        {
            _borrowRepo = borrowRepository;
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            return View(await _borrowRepo.GetAllBorrowsAsync());
        }

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var borrow = await _borrowRepo.GetBorrowByIdAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        [HttpGet("/Borrows/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var borrow = await _borrowRepo.GetBorrowByIdAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            return View(borrow);
        }

        [HttpGet("/Borrows/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var borrow = await _borrowRepo.GetBorrowByIdAsync(id);
            return View();
        }

        [HttpPost("/Borrows/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                await _borrowRepo.DeleteBorrow(id);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpPost("/Borrows/AddBorrow/{productId}")]
        public async Task<IActionResult> AddBorrow(int productId)
        {
            
            Borrow newBorrow = new Borrow();
            
            await _borrowRepo.AddBorrow(newBorrow, productId);

            return View(newBorrow);
        }

    }
}

