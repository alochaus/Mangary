using Microsoft.AspNetCore.Mvc;
using Mangary.Models;
using Mangary.ViewModels.Manager;
using Mangary.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Mangary.DAL;

namespace Mangary.Controllers
{
	[Authorize(Roles = "Admin, Manager")]
	public class ManagerController : Controller
	{
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly AppDbContext dbContext;
		private readonly IProductRepository productRepository;

		public ManagerController(
			IWebHostEnvironment hostingEnvironment,
			AppDbContext dbContext,
			IProductRepository productRepository
		)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.dbContext = dbContext;
			this.productRepository = productRepository;
		}

		[HttpGet]
		public IActionResult CreateProduct() => View();

		[HttpPost]
		public IActionResult CreateProduct(CreateProductViewModel model)
		{
			if(ModelState.IsValid)
			{
				string FileName = null;
				uint[] ArrCategories = new uint[3];

				if(model.Photo != null)
				{
					string FolderPath = Path.Combine(hostingEnvironment.WebRootPath, "UploadedPhotos");

					FileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";

					string FilePath = Path.Combine(FolderPath, FileName);

					model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
				}

				Guid ProductId = Guid.NewGuid();

				List<ProductCategories> productCategories = new List<ProductCategories>();

				productCategories.Add(new ProductCategories
				{
					ProductId = ProductId,
					CategoryId = model.Category1 
				});	

				productCategories.Add(new ProductCategories
				{
					ProductId = ProductId,
					CategoryId = model.Category2 
				});	

				productCategories.Add(new ProductCategories
				{
					ProductId = ProductId,
					CategoryId = model.Category3 
				});	

				foreach(ProductCategories category in productCategories)
				{
					dbContext.Add(category);
					dbContext.SaveChanges();
				}

				Product product = new Product()
				{
					Name 		= model.Name,
					ProductId	= ProductId,
					Description	= model.Description,
					Price		= model.Price,
					Quantity	= model.Quantity,
					PhotoPath	= FileName
				};

				dbContext.Add(product);
				dbContext.SaveChanges();
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpGet("[Controller]/[action]/{ProductId}")]
		public IActionResult EditProduct(Guid ProductId)
		{
			Product product = productRepository.GetProductById(ProductId);
			Categories?[] categories = dbContext.ProductCategories.Where(x => x.ProductId == product.ProductId).Select(y => y.CategoryId).ToArray();

			EditProductViewModel model = new EditProductViewModel()
			{
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Quantity = product.Quantity,
				Category1 = categories[0],
				Category2 = categories[1],
				Category3 = categories[2],
				PhotoPath = product.PhotoPath,
				ProductId = product.ProductId
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult EditProduct(EditProductViewModel model)
		{
			string FileName = null;
			Product product = productRepository.GetProductById(model.ProductId);
			if(product != null)
			{
				product.Name = model.Name;
				product.Description = model.Description;
				product.Price = model.Price;
				product.Quantity = model.Quantity;

				if(model.Photo == null)
				{
					product.PhotoPath = model.PhotoPath;
				}
				else
				{
					string FolderPath = Path.Combine(hostingEnvironment.WebRootPath, "UploadedPhotos");

					FileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";

					string FilePath = Path.Combine(FolderPath, FileName);

					model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));

					product.PhotoPath = FileName;
				}

				productRepository.UpdateProduct(product);
				productRepository.Save();

				List<ProductCategories> categories = dbContext.ProductCategories.Where(x => x.ProductId == model.ProductId).ToList();
				categories[0].CategoryId = model.Category1;
				categories[1].CategoryId = model.Category2;
				categories[2].CategoryId = model.Category3;

				dbContext.ProductCategories.UpdateRange(categories);
				dbContext.SaveChanges();
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public IActionResult DeleteProduct(Guid ProductId)
		{
			productRepository.DeleteProduct(ProductId);
			Product ProductInProducts = productRepository.GetProductById(ProductId);
			//IQueryable<Product> ProductInProducts = dbContext.Products.Where(x => x.ProductId == ProductId);
			if(ProductInProducts != null)
			{
				IQueryable<Cart> ProductInCart = dbContext.Cart.Where(x => x.ProductId == ProductId);

				productRepository.DeleteProduct(ProductInProducts);
				//dbContext.Products.RemoveRange(ProductInProducts);
				dbContext.Cart.RemoveRange(ProductInCart);
				dbContext.SaveChanges();
			}
			return RedirectToAction("Index", "Home");
		}
	}
}