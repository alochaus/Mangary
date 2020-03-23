using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mangary.Data;
using Microsoft.AspNetCore.Mvc;
using Mangary.Models;
using Mangary.Services;
using Mangary.ViewModels.Products;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Mangary.Controllers
{
	public class ProductController : Controller
	{
		private readonly AppDbContext dbContext;
		private readonly UserManager<IdentityUser> userManager;

		public ProductController(
			AppDbContext dbContext,
			UserManager<IdentityUser> userManager
		)
		{
			this.dbContext = dbContext;
			this.userManager = userManager;
		}

		[HttpGet("[controller]/[action]")]
		public IActionResult Category(string id)
		{
			string[] CategoryName = Enum.GetNames(typeof(Categories));
			List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();

			if(String.IsNullOrEmpty(id))
			{
				categoryViewModel.Add(
					new CategoryViewModel
					{
						Name="None",
						IsSelected=false
					}
				);

				for(int i=1; i<CategoryName.Count(); i++)
				{
					categoryViewModel.Add(
						new CategoryViewModel
						{
							Name = CategoryName[i],
							IsSelected = false
						}
					);
				}
				return View(categoryViewModel);
			}
			string[] categories = id.Split("_");
			List<int> CategoryId = new List<int>();
			foreach(string category in categories)
				CategoryId.Add(ProductServices.CategoryParser<Categories>(category));
			
			List<Product> MangaList = new List<Product>();
			MangaList.AddRange(ProductServices.GetProducts(
				dbContext, ProductServices.GetProductIdByCategoryId(
					dbContext, CategoryId[0]
				)
			));

			categoryViewModel.Add(
				new CategoryViewModel
				{
					MangaList = MangaList,
					Name = CategoryName[0],
					IsSelected = false
				}
			);

			for(int i=1; i<CategoryName.Count(); i++)
			{
				categoryViewModel.Add(
					new CategoryViewModel
					{
						Name = CategoryName[i],
						IsSelected = false
					}
				);
			}

			return View(categoryViewModel);
		}

		[HttpGet("Search")]
		public IActionResult Search(string pattern)
		{
			if(string.IsNullOrEmpty(pattern)) return RedirectToAction("Index", "Home");

			string Pattern = StringServices.Cleaner(pattern);

			List<Product> ProductList = new List<Product>();

			IQueryable<Product> Temp = ProductServices.SearchFor(dbContext, Pattern);
			ProductList.AddRange(Temp);

			return View(ProductList);
		}

		[HttpPost]
		public IActionResult CategorySearch(List<CheckBoxModel> model)
		{
			List<string> SelectedCategories = model.Where(x => x.IsSelected).Select(y => y.Name).ToList();

			StringBuilder CategoryString = new StringBuilder("");

			for(int i=0; i<SelectedCategories.Count(); i++)
			{
				CategoryString.Append(SelectedCategories[i] + "_");
			}
			CategoryString.Remove(CategoryString.Length - 1, 1);
			
			return RedirectToAction("Category", "Product", new { id = CategoryString });
		}

		[HttpGet("[controller]/{ProductId}")]
		public async Task<IActionResult> ProductsPage(Guid ProductId)
		{
			Product product = new Product();
			product = dbContext.Products.Where(x => x.ProductId == ProductId).SingleOrDefault();

			IdentityUser User = await userManager.GetUserAsync(HttpContext.User);
			try
			{
				ViewBag.HasManagerRole = await userManager.IsInRoleAsync(User, "Manager");
			}
			catch
			{
				ViewBag.HasManagerRole = false;
			}

			return View(product);
		}
	}
}