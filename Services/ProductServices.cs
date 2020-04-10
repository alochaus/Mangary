using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.Data;
using Mangary.Models;
using Microsoft.EntityFrameworkCore;

namespace Mangary.Services
{
	public static class ProductServices
	{
		public static IQueryable<Guid> GetProductIdByCategoryId(AppDbContext dbContext, int id)
		{
			return dbContext.ProductCategories.Where(x => x.CategoryId == (Categories?)id).Select(x => x.ProductId);
		}

		public static IQueryable<Guid> GetProductIdByCategoryId(AppDbContext dbContext, string name)
		{
			int id = CategoryParser<Categories>(name);
			return GetProductIdByCategoryId(dbContext, id);
		}

		public static IQueryable<Product> GetProduct(AppDbContext dbContext, Guid ProductId)
		{
			return dbContext.Products.Where(x => x.ProductId == ProductId);
		}

		public static IQueryable<Product> GetProducts(AppDbContext dbContext, IQueryable<Guid> ProductId)
		{
			return dbContext.Products.Where(x => ProductId.Any(y => x.ProductId == y));
		}

		public static IEnumerable<Product> GetLatestMangaAdded(AppDbContext dbContext, int Page, int NumOfProds)
		{
			int skip = (Page - 1) * NumOfProds;
			return dbContext.Products.AsEnumerable().OrderByDescending(x => x.Id).Skip(skip).Take(NumOfProds);
		}

		public static int CategoryParser<T>(string name) where T: new()
		{
			return (int)Enum.Parse(typeof(T), name);
		}

		public static IQueryable<Product> SearchFor(AppDbContext dbContext, string pattern)
		{
			return dbContext.Products.Where(x => EF.Functions.Like(x.Name, $"%{pattern}%"));
		}
	}
}