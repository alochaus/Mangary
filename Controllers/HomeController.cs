using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mangary.Models;
using Mangary.Data;
using Mangary.ViewModels;

namespace Mangary.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext dbContext;

		public HomeController(
			ILogger<HomeController> logger,
			AppDbContext dbContext
		)
		{
			_logger = logger;
			this.dbContext = dbContext;
		}

		public IActionResult Index()
		{
			List<Product> LatestProductsAdded = new List<Product>();
			LatestProductsAdded.AddRange(dbContext.Products.OrderByDescending(x => x.Id).Take(5));

			HomeViewModel homeViewModel = new HomeViewModel()
			{
				LatestProductsAdded = LatestProductsAdded
			};

			return View(homeViewModel);
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
