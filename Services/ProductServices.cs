using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.Data;
using Mangary.Models;
using Microsoft.EntityFrameworkCore;

namespace Mangary.Services
{
	public class ProductServices
	{
		private readonly AppDbContext dbContext;

		public ProductServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public List<ProductCategories> GetProductCategories(int id)
		{
			return dbContext.ProductCategories.FromSqlRaw($"SELECT * FROM ProductCategories WHERE CategoryId = {id}").ToList();
		}

		public List<ProductCategories> GetProductCategories(string name)
		{
			int id = CategoryParser<Categories>(name);
			return dbContext.ProductCategories.FromSqlRaw($"SELECT * FROM ProductCategories WHERE CategoryId = {id}").ToList();
		}

		public IQueryable<Product> GetProduct(Guid ProductId)
		{
			return dbContext.Products.FromSqlRaw($"SELECT * FROM Products WHERE ProductId = \"{ProductId.ToString().ToUpper()}\"");
		}

		public int CategoryParser<T>(string name) where T: new()
		{
			return (int)Enum.Parse(typeof(T), name);
		}

		public IQueryable<Product> SearchFor(string pattern)
		{
			Console.WriteLine("\n\n\n\n\n\n\n\n\n");
			Console.WriteLine($"SELECT * FROM Products WHERE Name LIKE \"%{pattern}%\"");
			Console.WriteLine("\n\n\n\n\n\n\n\n\n");
			return dbContext.Products.FromSqlRaw($"SELECT * FROM Products WHERE Name LIKE \"%{pattern}%\"");
		}
	}
}