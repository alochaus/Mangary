using System;
using System.ComponentModel.DataAnnotations;

namespace Mangary.Models
{
	public class Product
	{
		[Required]
		public int			Id			{ get; set; }

		[Required]
		public Guid			ProductId	{ get; set; }

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

		public string		PhotoPath	{ get; set; }
	}
}