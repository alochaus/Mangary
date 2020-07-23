using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mangary.Models;
using Mangary.Services;
using Mangary.ViewModels.Products;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Mangary.DAL;

namespace Mangary.Controllers
{
	public class ProductController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly IProductRepository productRepository;
		private readonly ICategoryServices categoryServices;

		public ProductController(
			UserManager<IdentityUser> userManager,
			IProductRepository productRepository,
			ICategoryServices categoryServices
		)
		{
			this.userManager = userManager;
			this.productRepository = productRepository;
			this.categoryServices = categoryServices;
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

			List<Product> MangaList = categoryServices.Search(ref id);

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

			//List<Product> ProductList = new List<Product>();

			IEnumerable<Product> Result = productRepository.Search(Pattern);
			// IQueryable<Product> Temp = ProductServices.SearchFor(dbContext, Pattern);
			//ProductList.AddRange(Result);

			return View(Result);
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
			Product product = productRepository.GetProductById(ProductId);

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

		[HttpGet("LatestMangaAdded/{Page}")]
		public IActionResult LatestProductsAdded(int Page)
		{
			int NumOfProds = 10;
			if(!(Page >= 1)) return RedirectToAction("Index", "Home");
			ViewBag.Page = Page;
			ViewBag.DisplayPrev = Page > 1;
			ViewBag.DisplayNext = productRepository.Count() > ((Page - 1) * NumOfProds) + NumOfProds;
			IEnumerable<Product> Products = productRepository.GetLatestProductsAdded(Page, NumOfProds);
			//IEnumerable<Product> Products = ProductServices.GetLatestMangaAdded(dbContext, Page, NumOfProds);
			return View(Products);
		}
	}
}