using System.ComponentModel.DataAnnotations;

namespace WebApplication5.ViewModels
{
	public class LoginViewModel
	{
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
