using System.ComponentModel.DataAnnotations;

namespace Mangary.ViewModels.Account
{
	public class SignInViewModel
	{
		[Required]
		[Display(Name="Username")]
		public string Username { get; set; }

		[Required]
		[Display(Name="Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name="Remember me")]
		public bool RememberMe { get; set; }
	}
}