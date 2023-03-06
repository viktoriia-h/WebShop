using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data.Models;
using WebApplication5.Data;
using WebApplication5.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext dbContext)
		{
			_context = dbContext;
		}

        public PartialViewResult SearchBook(string searchText)
        {
            var books = _context.Books.ToList();

            if (searchText != null)
            {
				var result = books.Where(x => x.Name.ToLower().Contains(searchText.ToLower()) || x.Author.ToLower().Contains(searchText.ToLower())).ToList();
				var resultView = result.Select(x => Helper.ToViewModel(x)).ToList();
				return PartialView("/Pages/Customer/_GridBooks.cshtml", resultView);
			}
			return PartialView("/Pages/Customer/_GridBooks.cshtml", books.Select(x => Helper.ToViewModel(x)).ToList());

		}

		public async Task<IActionResult> ShowCustomerPage(LoginViewModel model)
		{
			var books = _context.Books.ToList();
			var bookViewModels = books.Select(x => Helper.ToViewModel(x)).ToList();
			return View("~/Pages/Customer/UserPage.cshtml", bookViewModels);
		}

        public async Task<IActionResult> ShowBookSearchPage(LoginViewModel model)
        {
            var books = _context.Books.ToList();
            var bookViewModels = books.Select(x => Helper.ToViewModel(x)).ToList();
            return View("~/Pages/Customer/SearchAndFiltersPage.cshtml", bookViewModels);
        }

        public async Task<IActionResult> ShowBookPage(LoginViewModel model)
        {
            var books = _context.Books.ToList();
            var bookViewModels = books.Select(x => Helper.ToViewModel(x)).ToList();
            return View("~/Pages/Customer/UserPage.cshtml", bookViewModels);
        }

        public IActionResult RedirectToSwowBookView(Guid Id)
        {
            var existingBook = _context.Books.First(x => x.Id == Id);

            var stream = new MemoryStream(existingBook.Picture);
            IFormFile file = new FormFile(stream, 0, existingBook.Picture.Length, "name", "fileName");
            var book = Helper.ToViewModel(existingBook);
            book.Picture = file;


            return View("~/Pages/Customer/BookShowPage.cshtml", book);
        }
    }
}
