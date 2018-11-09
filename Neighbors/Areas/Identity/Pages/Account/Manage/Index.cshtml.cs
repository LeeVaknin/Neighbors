using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Neighbors.Data;
using Neighbors.Models;
using Neighbors.Services.DAL;

namespace Neighbors.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICategoriesRepository _catRepo;
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ICategoriesRepository categoriesRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _catRepo = categoriesRepository;
        }

        public async Task<ICollection<Product>> GetMyProducts()
        {
            var strUserId = _signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var response = (await _signInManager.UserManager.FindByIdAsync(strUserId)).MyProducts;
            return response;
        }

        // Get all the products that I borrowed from others
        public async Task<ICollection<Borrow>> GetMyBorrowedProducts()
        {
            var strUserId = _signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = (await _signInManager.UserManager.FindByIdAsync(strUserId)).MyBorrowed;
            return response;
        }

        public async Task<ICollection<Category>> GetAllCategories()
        {
            return await _catRepo.GetAllCategories();
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

			[Display(Name = "First Name")]
			[StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
			public string FirstName { get; set; }

			[Display(Name = "Last Name")]
			[StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
			public string LastName { get; set; }

			[Display(Name = "Street Address")]
			[StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
			public string Address { get; set; }

			[Display(Name = "City")]
			[StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
			public string City { get; set; }
		}

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //var userName = await _userManager.GetUserNameAsync(user);
            //var email = await _userManager.GetEmailAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = user.UserName;

            Input = new InputModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Address = user.Address,
				City = user.City,
            };

            //IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound();
			}

			if (user.Email != Input.Email) { user.Email = Input.Email; }
			if (user.PhoneNumber != Input.PhoneNumber) { user.PhoneNumber = Input.PhoneNumber; }
			if (user.FirstName != Input.FirstName) { user.FirstName = Input.FirstName; }
			if (user.LastName != Input.LastName) { user.LastName = Input.LastName; }
			if (user.Address != Input.Address) { user.Address = Input.Address; }
			if (user.City != Input.City) { user.City = Input.City; }
			var setPhoneResult = await _userManager.UpdateAsync(user);
            if (!setPhoneResult.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
