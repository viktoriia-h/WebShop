using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data;
using WebApplication5.Data.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		public AdminController(ApplicationDbContext dbContext)
		{
			_context = dbContext;
		}

		public async Task<IActionResult> ShowAdminPage(LoginViewModel model)
		{
			var books = _context.Books.ToList();
			var bookViewModels = books.Select(x => Helper.ToViewModel(x)).ToList();
			return View("~/Pages/Admin/AdminPage.cshtml", bookViewModels);
		}
		public IActionResult GenerationKeys()
		{
			var key = new BuyKeys()
			{
				Name = Helper.RandomString(5)
			};
			_context.BuyKeys.Add(key);
			_context.SaveChanges();
			TempData["Message"] = $"Your key: {key.Name}";
			
			return RedirectToAction("ShowAdminPage", "Admin");
		}


	}
}
