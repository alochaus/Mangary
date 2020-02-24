using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mangary.Models;
using Mangary.Data;
using Mangary.Services;
using Mangary.ViewModels;

namespace Mangary.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext dbContext;

		public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			this.dbContext = dbContext;
		}

		public IActionResult Index()
		{
			List<Product> LatestProductsAdded = new List<Product>();
			LatestProductsAdded = dbContext.Products.OrderByDescending(x => x.Id).Take(3).ToList();

			HomeViewModel homeViewModel = new HomeViewModel()
			{
				LatestProductsAdded = LatestProductsAdded
			};

			return View(homeViewModel);
			/*
			ProductServices productServices = new ProductServices(dbContext);

			List<ProductCategories> ActionId = productServices.GetProductCategories("Action");
			List<ProductCategories> PsychologicalId = productServices.GetProductCategories("Psychological");

			List<Product> Action = new List<Product>();
			List<Product> Psychological = new List<Product>();

			foreach(ProductCategories row in ActionId)
			{
				IQueryable<Product> Temp = productServices.GetProduct(row.ProductId);
				Action.AddRange(Temp);
			}

			foreach(ProductCategories row in PsychologicalId)
			{
				IQueryable<Product> Temp = productServices.GetProduct(row.ProductId);
				Psychological.AddRange(Temp);
			}

			HomeViewModel model = new HomeViewModel()
			{
				Action = Action,
				Psychological = Psychological
			};

			return View(model);
			*/
		}
		public IActionResult About()	=> View();
		public IActionResult Contact()	=> View();
		public IActionResult Privacy()	=> View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
