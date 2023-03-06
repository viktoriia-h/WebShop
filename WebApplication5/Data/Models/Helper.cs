using WebApplication5.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data;
using WebApplication5.Data.Models;

namespace WebApplication5.Data.Models
{
    public class Helper
    {
        public static BookViewModel ToViewModel(Book book)
        {
            return new BookViewModel()
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                Author = book.Author,
                Year = book.Year,
                Genre = book.Genre
            };
        }

		private static Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}

}
