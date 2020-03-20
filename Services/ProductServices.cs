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
			return dbContext.ProductCategories.Where(x => x.CategoryId.Equals(id)).Select(x => x.ProductId);
//			return dbContext.ProductCategories.FromSqlRaw($"SELECT * FROM ProductCategories WHERE CategoryId = {id}").ToList();
		}

		public static IQueryable<Guid> GetProductIdByCategoryId(AppDbContext dbContext, string name)
		{
			int id = CategoryParser<Categories>(name);
			return dbContext.ProductCategories.Where(x => x.CategoryId.Equals(id)).Select(x => x.ProductId);
//			return dbContext.ProductCategories.FromSqlRaw($"SELECT * FROM ProductCategories WHERE CategoryId = {id}").ToList();
		}

		public static IQueryable<Product> GetProduct(AppDbContext dbContext, Guid ProductId)
		{
			return dbContext.Products.Where(x => x.ProductId == ProductId);
//			return dbContext.Products.FromSqlRaw($"SELECT * FROM Products WHERE ProductId = \"{ProductId.ToString().ToUpper()}\"");
		}

		public static int CategoryParser<T>(string name) where T: new()
		{
			return (int)Enum.Parse(typeof(T), name);
		}

		public static IQueryable<Product> SearchFor(AppDbContext dbContext, string pattern)
		{
			Console.WriteLine("\n\n\n\n\n\n\n\n\n");
			Console.WriteLine($"SELECT * FROM Products WHERE Name LIKE \"%{pattern}%\"");
			Console.WriteLine("\n\n\n\n\n\n\n\n\n");
			return dbContext.Products.Where(x => EF.Functions.Like(x.Name, $"%{pattern}%"));
//			return dbContext.Products.FromSqlRaw($"SELECT * FROM Products WHERE Name LIKE \"%{pattern}%\"");
		}
	}
}