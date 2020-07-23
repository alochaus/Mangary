using System;
using System.Collections.Generic;
using System.Linq;
using Mangary.DAL;
using Mangary.Models;

namespace Mangary.Services
{
	public class CategoryServices : ICategoryServices
	{
		public readonly IProductRepository _product_repository;

		public CategoryServices(IProductRepository product_repository)
		{
			_product_repository = product_repository;
		}

		public List<Product> Search(ref string id)
		{
			string[] array_of_categories = id.Split("_");
			List<int> list_of_categories = CreateListOfCategories(ref array_of_categories);
			List<Product> manga_list = FetchProductsMatchingListOfCategories(ref list_of_categories);

			return manga_list;	
		}

		private List<Product> FetchProductsMatchingListOfCategories(ref List<int> list_of_categories)
		{
			List<Product> manga_list = new List<Product>();

			manga_list.AddRange(
				_product_repository.GetProductById(
					_product_repository.GetProductIdByCategoryId(list_of_categories[0])
				)
			);

			for(int i=1; i < list_of_categories.Count(); i++)
			{
				manga_list = manga_list.Intersect(
					_product_repository.GetProductById(
						_product_repository.GetProductIdByCategoryId(list_of_categories[i])
					)
				).ToList();
			}

			return manga_list;
		}

		private int ConvertCategoryToInteger(string category)
		=> (int)Enum.Parse(typeof(Categories), category);

		private List<int> CreateListOfCategories(ref string[] array_of_categories)
		{
			List<int> list = new List<int>();

			foreach(string category in array_of_categories)
				list.Add(ConvertCategoryToInteger(category));

			return list;
		}
	}
}