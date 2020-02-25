using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mangary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			if(string.IsNullOrEmpty(id))
			{
				string[] CategoryName = Enum.GetNames(typeof(Categories));

				List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>()
				{
					new CategoryViewModel
					{
						Name = "None",
						IsSelected = false
					}
				};

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

			// SELECT ProductId FROM ProductCategories WHERE ProductId IN (SELECT ProductId FROM ProductCategories WHERE CategoryId = 1) AND CategoryId = 2;
			string Query = "ZXCV SELECT ProductId FROM ProductCategories WHERE CategoryId = CATEGORIES_PARAM VBNM";

			Regex rgx = new Regex(@"^[a-z_A-Z]+$");
			Match matches = rgx.Match(id);
			ProductServices productServices = new ProductServices(dbContext);

			if(rgx.IsMatch(id))
			{
				if(categories.Length == 1)
				{
					int CategoryId = productServices.CategoryParser<Categories>(categories[0]);
					Query = Query.Replace("CATEGORIES_PARAM", CategoryId.ToString());
					Query = Query.Replace("ZXCV SELECT ProductId", "SELECT *");
					Query = Query.Replace(" VBNM", string.Empty);
				}
				else
				{
					for(int i=0;i<categories.Length;i++)
					{
						int CategoryId = productServices.CategoryParser<Categories>(categories[i]);
						Query = Query.Replace("CATEGORIES_PARAM", CategoryId.ToString());
						Query = Query.Replace("ZXCV ", "ZXCV SELECT ProductId FROM ProductCategories WHERE ProductId IN (");
						Query = Query.Replace(" VBNM", ") AND CategoryId = CATEGORIES_PARAM VBNM");	
					}
					Query = Query.Replace("ZXCV SELECT ProductId FROM ProductCategories WHERE ProductId IN (SELECT ProductId", "SELECT *");
					Query = Query.Replace(") AND CategoryId = CATEGORIES_PARAM VBNM", string.Empty);
				}

				List<ProductCategories> rows = dbContext.ProductCategories.FromSqlRaw(Query).ToList();
				List<Product> ProductList = new List<Product>();

				foreach(ProductCategories row in rows)
				{
					IQueryable<Product> Temp = dbContext.Products.FromSqlRaw($"SELECT * FROM Products WHERE ProductId = \"{row.ProductId.ToString().ToUpper()}\"");
					ProductList.AddRange(Temp);
				}

				string[] CategoryName = Enum.GetNames(typeof(Categories));

				List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>()
				{
					new CategoryViewModel
					{
						MangaList = ProductList,
						Name = CategoryName[0],
						IsSelected = false
					}
				};

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
			return RedirectToAction("Index", "Home");
		}

		[HttpGet("Search")]
		public IActionResult Search(string pattern)
		{
			if(string.IsNullOrEmpty(pattern)) return RedirectToAction("Index", "Home");

			StringServices stringServices = new StringServices();
			string Pattern = stringServices.Cleaner(pattern);

			List<Product> ProductList = new List<Product>();

			ProductServices productServices = new ProductServices(dbContext);
			IQueryable<Product> Temp = productServices.SearchFor(Pattern);
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
				Console.Write("\n\n\n\n\n{0}\n\n\n\n\n", ViewBag.HasManagerRole.ToString());
			}
			catch
			{
				ViewBag.HasManagerRole = false;
			}

			return View(product);
		}
	}
}