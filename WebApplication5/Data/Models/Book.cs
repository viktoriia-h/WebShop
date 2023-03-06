using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using WebApplication5.ViewModels;

namespace WebApplication5.Data.Models
{
	public class Book
	{
        public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public byte[] Pdf { get; set; }
		public byte[] Picture { get; set; }
		public int Year { get; set; }
		public string Genre { get; set; }
	}
}
