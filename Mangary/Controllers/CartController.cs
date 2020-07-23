using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mangary.DAL;
using Mangary.Data;
using Mangary.Models;
using Mangary.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mangary.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly AppDbContext dbContext;
		private readonly IProductRepository productRepository;
		private readonly ICartRepository cartRepository;

		public CartController(
			UserManager<IdentityUser> userManager,
			AppDbContext dbContext,
			IProductRepository productRepository,
			ICartRepository cartRepository
		)
		{
			this.userManager = userManager;
			this.dbContext = dbContext;
			this.productRepository = productRepository;
			this.cartRepository = cartRepository;
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(Guid ProductId)
		{
			IdentityUser User = await userManager.GetUserAsync(HttpContext.User);

 			bool ProductExists = productRepository.GetProductById(ProductId) != null;
			bool IsAlreadyInCart = cartRepository.IsProductInCart(ProductId, User.Email);
			//bool IsAlreadyInCart = dbContext.Cart.Where(x => x.ProductId == ProductId && x.Email == User.Email).Any();

			if(User != null && ProductExists && !IsAlreadyInCart)
			{
				Cart cart = new Cart
				{
					Email = User.Email,
					ProductId = ProductId
				};

				cartRepository.AddProductToCart(cart);
				//dbContext.Add(cart);
				cartRepository.Save();
				//dbContext.SaveChanges();
			}

			return RedirectToAction("Index", "Cart");
		}

		[HttpGet("[controller]/[action]/{ProductId}")]
		public async Task<IActionResult> DeleteFromCart(Guid ProductId)
		{
			IdentityUser User = await userManager.GetUserAsync(HttpContext.User);
			try
			{
				Cart CartItem = cartRepository.GetCartItem(ProductId, User.Email);
				//IQueryable<Cart> CartItem = dbContext.Cart.Where(x => x.ProductId == ProductId && x.Email == User.Email);
				if(User != null)
				{
					cartRepository.DeleteProductFromCart(CartItem);
					//dbContext.Cart.RemoveRange(CartItem);
					cartRepository.Save();
					//dbContext.SaveChanges();
				}

				return RedirectToAction("Index", "Cart");
			}
			catch(System.Exception error)
			{
				Console.WriteLine("\n\n\n\n\n\n\n\n");
				Console.WriteLine(error.ToString());
				Console.WriteLine("\n\n\n\n\n\n\n\n");
				return RedirectToAction("About", "Home");
			}

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IdentityUser User = await userManager.GetUserAsync(HttpContext.User);

			IEnumerable<Guid> GuidList = cartRepository.GetProducts(User.Email);
			//IEnumerable<Guid> GuidList = dbContext.Cart.Where(x => x.Email == User.Email).Select(x => x.ProductId).ToList();
			List<Product> Products = new List<Product>();

			IEnumerable<Product> prod = productRepository.GetProductById(GuidList);
			//List<Product> prod = dbContext.Products.Where(x => cart.Contains(x.ProductId)).ToList();
			Products.AddRange(prod); 
			
			List<CartViewModel> model = new List<CartViewModel>();

			foreach(Product product in Products)
			{
				CartViewModel cartViewModel = new CartViewModel()
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Price = product.Price,
					PhotoPath = product.PhotoPath
				};

				model.Add(cartViewModel); 
			}

			return View(model);
		}
	}
}