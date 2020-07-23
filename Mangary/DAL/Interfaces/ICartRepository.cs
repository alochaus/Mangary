using System;
using System.Collections.Generic;
using Mangary.Models;

namespace Mangary.DAL
{
	public interface ICartRepository : IDisposable
	{
		public void AddProductToCart(Cart cart);
		public void DeleteProductFromCart(Cart cart);
		public bool IsProductInCart(Guid ProductId, string Email);
		public IEnumerable<Guid> GetProducts(string Email);
		public Cart GetCartItem(Guid ProductId, string Email);
		void Save();
	}
}