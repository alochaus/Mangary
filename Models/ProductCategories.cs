using Mangary.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangary.Models
{
	public class ProductCategories
	{
		public int			Id			{ get; set; }

		[Required]
		public Guid			ProductId	{ get; set; }
		
		[Required]
		public Categories?	CategoryId	{ get; set; }
	}
}