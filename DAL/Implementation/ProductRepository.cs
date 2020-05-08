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

		public void DeleteProduct(Product product)
		{
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

		public IEnumerable<Product> GetProductById(IEnumerable<Guid> GuidList)
		{
			return dbContext.Products.Where(x => GuidList.Contains(x.ProductId)).ToList();
		}

		public IEnumerable<Product> GetLatestProductsAdded(int Page, int NumOfProds)
		{
			int skip = (Page - 1) * NumOfProds;
			return dbContext.Products.AsEnumerable().OrderByDescending(x => x.Id).Skip(skip).Take(NumOfProds);
		}

		public IEnumerable<Guid> GetProductIdByCategoryId(int id)
		{
			return dbContext.ProductCategories.Where(x => x.CategoryId == (Categories?)id).Select(x => x.ProductId).ToList();
		}

		public void InsertProduct(Product product)
		{
			dbContext.Products.Add(product);
		}

		public IEnumerable<Product> Search(string pattern)
		{
			return dbContext.Products.Where(x => EF.Functions.Like(x.Name, $"%{pattern}%")).ToList();
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