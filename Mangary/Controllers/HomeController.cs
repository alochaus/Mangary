using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mangary.Models;
using Mangary.ViewModels;
using Mangary.DAL;

namespace Mangary.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductRepository productRepository;

		public HomeController(
			ILogger<HomeController> logger,
			IProductRepository productRepository
		)
		{
			_logger = logger;
			this.productRepository = productRepository;
		}

		public IActionResult Index()
		{
			List<Product> LatestProductsAdded = new List<Product>();
			LatestProductsAdded.AddRange(productRepository.GetLatest(5));

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
