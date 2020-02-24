using Microsoft.AspNetCore.Http;
using Mangary.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Mangary.ViewModels.Manager
{
	public class EditProductViewModel
	{
		public Product		product		{ get; set; }

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

		public IFormFile	Photo		{ get; set; }

		[Required]
		public string		PhotoPath	{ get; set; }

		[Required]
		public Guid			ProductId	{ get; set; }
	}
}
