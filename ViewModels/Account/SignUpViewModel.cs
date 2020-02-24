using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Mangary.ViewModels.Account
{
	public class SignUpViewModel
	{
		[Required]
		[StringLength(15)]
		[Display(Name="Username")]
		[Remote(action: "VerifyUsername", controller: "Account")]
		public string Username { get; set; }

		[Required]
		[Display(Name="Email")]
		[EmailAddress]
		[Remote(action: "VerifyEmail", controller: "Account")]
		public string Email { get; set; }

		[Required]
		[Display(Name="Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[Display(Name="Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage="Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		[Display(Name="Remember me")]
		public bool RememberMe { get; set; }
	}
}