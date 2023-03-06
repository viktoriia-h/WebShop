namespace WebApplication5.ViewModels
{
	public class BookViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public IFormFile Pdf { get; set; }
		public IFormFile Picture { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

    }
}
