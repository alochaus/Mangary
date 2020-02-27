using System;

namespace Mangary.ViewModels.Cart
{
	public class CartViewModel
	{
		public Guid ProductId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public uint Quantity { get; set; } = 1;

		public string PhotoPath { get; set; }
	}
}