using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Net.Mime;
using System.Xml.Linq;
using WebApplication5.Data;
using WebApplication5.Data.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult RedirectToAddBookView()
        {
            return View("~/Pages/Admin/AddBookPage.cshtml");
        }

        public IActionResult RedirectToEditBookView(Guid Id)
        {
            var existingBook = _context.Books.First(x => x.Id == Id);
            return View("~/Pages/Admin/EditBookPage.cshtml", Helper.ToViewModel(existingBook));
        }

        public IActionResult ShowImage(Guid Id)
        {
            var existingBook = _context.Books.First(x => x.Id == Id);

            return File(existingBook.Picture, "image/jpg");
        }
        public IActionResult ShowPdf(Guid Id)
        {
            var existingBook = _context.Books.First(x => x.Id == Id);

            return File(existingBook.Pdf, "pdf");
        }

        public IActionResult DeleteBook(Guid Id)
        {
            var book = _context.Books.First(x => x.Id == Id);
            if (book == null)
            {
                throw new Exception($"Cann`t delete item with id {Id} since it doesn`t exist in DB");
            }
            _context.Books.Attach(book);
            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("ShowAdminPage", "Admin");

        }

        public IActionResult AddOrUpdateBook(BookViewModel model)
        {
            var existingBook = _context.Books.FirstOrDefault(x => x.Id == model.Id);

            if (existingBook != null)
            {
                existingBook.Author = model.Author;
                existingBook.Name = model.Name;
                existingBook.Description = model.Description;
                existingBook.Year = model.Year;
                existingBook.Genre = model.Genre;

                _context.SaveChanges();
                return RedirectToAction("ShowAdminPage", "Admin");
            }

            var book = new Book()
            {
                Id = model.Id,
                Name = model.Name,
                Author = model.Author,
                Description = model.Description,
                Year = model.Year,
                Genre = model.Genre
            };
            using (var target = new MemoryStream())
            {
                model.Pdf.CopyTo(target);
                book.Pdf = target.ToArray();
            }
            using (var target = new MemoryStream())
            {
                model.Picture.CopyTo(target);
                book.Picture = target.ToArray();
            }
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("ShowAdminPage", "Admin");
        }

        public ActionResult EditBook(Guid Id)
        {
            var book = _context.Books.First(x => x.Id == Id);
            _context.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("ShowAdminPage", "Admin");
        }

		public IActionResult DownloadPdf(Guid id)
		{
			var book =  _context.Books.FirstOrDefault(x => x.Id == id);
			if (book == null)
			{
				return NotFound();
			}
			else
			{
				byte[] byteArr = book.Pdf;
				string mimeType = "application/pdf";
				return new FileContentResult(byteArr, mimeType)
				{
					FileDownloadName = $"{book.Name}.pdf"
				};
			}
		}
        public IActionResult EnshureKey(IFormCollection collection, Guid bookId)
        {
            var key = collection["key"].ToString();
            if (key != null)
            {
				var buyKey = _context.BuyKeys.FirstOrDefault(x => x.Name == key);
                if (buyKey != null)
                {
                    _context.BuyKeys.Remove(buyKey);
                    _context.SaveChanges();
					return RedirectToAction("DownloadPdf", "Book", new { id = bookId });
				}
			}
			return RedirectToAction("RedirectToSwowBookView", "Home", new { id = bookId});

		}
	}
}
