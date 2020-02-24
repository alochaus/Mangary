using System.ComponentModel.DataAnnotations;

namespace Mangary.ViewModels.Admin
{
	public class RoleManagerViewModel
	{
		[Required]
		[Display(Name="Username")]
		public string Username { get; set; }
	}
}