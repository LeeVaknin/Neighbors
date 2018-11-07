using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Neighbors.Models;


namespace Neighbors.Controllers
{
	[AllowAnonymous]
	public class LoginController: Controller
	{
		private readonly SignInManager<User> _signInManager;

		public LoginController(SignInManager<User> signInManager)
		{
			_signInManager = signInManager;
		}

		[TempData]
		public string ErrorMessage { get; set; }
	
		[HttpPost("/Login")]
		public async Task<IActionResult> OnPostAsync([FromBody] Login loginInput)
		{
			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync(loginInput.Email, loginInput.Password, loginInput.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					//_logger.LogInformation("User logged in.");
					return Json(new { model = loginInput, error = "", isValid = true });
				}
				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = Url.Content("~/"), RememberMe = loginInput.RememberMe });
				}
				if (result.IsLockedOut)
				{
					//_logger.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}
			}

			// If we got this far, something failed, redisplay form
			return Json(new { model = loginInput, error = "Invalid login attempt.", isValid = false });
		}
	}

}
