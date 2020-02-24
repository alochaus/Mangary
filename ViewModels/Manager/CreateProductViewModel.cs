using Microsoft.AspNetCore.Http;
using Mangary.Models;
using System.ComponentModel.DataAnnotations;

namespace Mangary.ViewModels.Manager
{
	public class CreateProductViewModel
	{
		[Required]
		[MaxLength(100)]
		public string		Name		{ get; set; }

		[MaxLength(3000)]
		public string		Description	{ get; set; }

		[Required]
		[Range(0.0, double.MaxValue)]
		public decimal		Price		{ get; set; }

		[Required]
		[Range(0, uint.MaxValue, ErrorMessage = "Quantity has to be greater or equal to one.")]
		public uint			Quantity	{ get; set; }

		[Required]
		public Categories?	Category1	{ get; set; }

		[Required]
		public Categories?	Category2	{ get; set; }

		[Required]
		public Categories?	Category3	{ get; set; }

		[Required]
		public IFormFile	Photo		{ get; set; }
	}
}