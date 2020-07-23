using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.DAL;
using Mangary.Data;
using Mangary.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Mangary.Services;

namespace Mangary.Tests
{
	public class TestCategorySearch
	{
		[Theory]
		[InlineData("Action_Adventure_Cars_Comedy_Dementia_Demons_Drama", 0)]
		[InlineData("Action_Adventure", 2)]
		[InlineData("Action", 3)]
		[InlineData("Adventure", 2)]
		[InlineData("Cars", 1)]
		public void TestCategories(string id, int expected)
		{
			//  arrange
			List<Product> list_of_products = new List<Product>
			{
				new Product
				{
					ProductId   = Guid.NewGuid(),
					Name        = "Drake",
					Description = "123",
					Price       = 4.99M,
					Quantity    = 30,
					PhotoPath   = "something/something.jpg"
				},
				new Product
				{
					ProductId   = Guid.NewGuid(),
					Name        = "Josh",
					Description = "123",
					Price       = 4.99M,
					Quantity    = 30,
					PhotoPath   = "something/something.jpg"
				},
				new Product
				{
					ProductId   = Guid.NewGuid(),
					Name        = "Megan",
					Description = "123",
					Price       = 4.99M,
					Quantity    = 30,
					PhotoPath   = "something/something.jpg"
				},
			};
			List<ProductCategories> list_of_categories = new List<ProductCategories>
			{
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(0).ProductId,
					CategoryId = Categories.Action // 3
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(0).ProductId,
					CategoryId = Categories.Adventure // 2
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(0).ProductId,
					CategoryId = Categories.Cars // 1
				},

				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(1).ProductId,
					CategoryId = Categories.Action
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(1).ProductId,
					CategoryId = Categories.Adventure
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(1).ProductId,
					CategoryId = Categories.Dementia // 1
				},

				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(2).ProductId,
					CategoryId = Categories.Action
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(2).ProductId,
					CategoryId = Categories.Demons
				},
				new ProductCategories
				{
					ProductId = list_of_products.ElementAt(2).ProductId,
					CategoryId = Categories.Drama
				},
			};

			DbContextOptions options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase("joshnichols")
				.Options;
			AppDbContext context = new AppDbContext(options);
			ProductRepository product_repository = new ProductRepository(context);
			CategoryServices category_services = new CategoryServices(product_repository);

			foreach(Product product in list_of_products)
				product_repository.InsertProduct(product);

			foreach(ProductCategories product_categories in list_of_categories)
				context.Add(product_categories);
			
			product_repository.Save();

			//  act
			List<Product> manga_list = category_services.Search(ref id);
			context.Database.EnsureDeleted();

			//  assert
			int number_of_results = manga_list.Count();
			Assert.True(number_of_results == expected, $"Expected {expected.ToString()} and recieved {number_of_results.ToString()}.");
		}
	}
}