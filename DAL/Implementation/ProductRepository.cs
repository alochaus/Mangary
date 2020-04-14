using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.Data;
using Mangary.Models;
using Microsoft.EntityFrameworkCore;

namespace Mangary.DAL
{
	public class ProductRepository : IProductRepository
	{
		private bool Disposed = false;
		private readonly AppDbContext dbContext;

		public ProductRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public void DeleteProduct(Guid ProductId)
		{
			Product product = dbContext.Products.Where(x => x.ProductId == ProductId).FirstOrDefault();
			dbContext.Products.Remove(product);
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

		public IEnumerable<Product> GetAll()
		{
			return dbContext.Products.ToList();
		}

		public Product GetProductById(Guid ProductId)
		{
			return dbContext.Products.Where(x => x.ProductId == ProductId).FirstOrDefault();
		}

		public void InsertProduct(Product product)
		{
			dbContext.Products.Add(product);
		}

		public void Save()
		{
			dbContext.SaveChanges();
		}

		public void UpdateProduct(Product product)
		{
			dbContext.Entry(product).State = EntityState.Modified;
		}

		public int Count()
		{
			return dbContext.Products.Count();
		}
	}
}