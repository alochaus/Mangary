using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.Data;
using Mangary.Models;

namespace Mangary.DAL
{
	public class CartRepository : ICartRepository
	{
		private bool Disposed = false;
		private readonly AppDbContext dbContext;

		public CartRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public void AddProductToCart(Cart cart)
		{
			dbContext.Add(cart);
		}

		public void DeleteProductFromCart(Cart cart)
		{
			dbContext.Cart.Remove(cart);
		}

		public bool IsProductInCart(Guid ProductId, string Email)
		{
			return dbContext.Cart.Where(x => x.ProductId == ProductId && x.Email == Email).Any();
		}

		public IEnumerable<Guid> GetProducts(string Email)
		{
			return dbContext.Cart.Where(x => x.Email == Email).Select(x => x.ProductId).ToList();

		}

		public Cart GetCartItem(Guid ProductId, string Email)
		{
			return dbContext.Cart.Where(x => x.ProductId == ProductId && x.Email == Email).SingleOrDefault();
		}

		public void Save()
		{
			dbContext.SaveChanges();
		}

		protected virtual void Dispose(bool Disposing)
		{
			if (!this.Disposed)
			{
				if (Disposing) dbContext.Dispose();
			}
			this.Disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

	}
}