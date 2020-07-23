using System;
using System.ComponentModel.DataAnnotations;

namespace Mangary.Models
{
	public class Cart
	{
		public int Id { get; set; }

		[Required]
		public string Email	{ get; set; }

		[Required]
		public Guid ProductId { get; set; }
	}
}