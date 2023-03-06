using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using WebApplication5.Data;
using WebApplication5.Data.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_context = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> LoginUser(LoginViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
			
			if (result.Succeeded && user.UserName == "admin")
			{
				return RedirectToAction("ShowAdminPage", "Admin");
			}
			return RedirectToAction("ShowCustomerPage", "Home");
		}        
    }
}
