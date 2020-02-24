using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mangary.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Mangary.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager
		)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult SignIn(string ReturnUrl) => View();
		[HttpGet]
		public IActionResult SignUp() => View();

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string ReturnUrl)
		{
			if(ModelState.IsValid)
			{
				var Result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

				if(Result.Succeeded)
				{
					if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
					{
						return Redirect(ReturnUrl);
					}
					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if(ModelState.IsValid)
			{
				IdentityUser User = new IdentityUser {
					UserName = model.Username,
					Email = model.Email
				};
				try
				{
					IdentityResult Result = await userManager.CreateAsync(User, model.Password);
					if(Result.Succeeded)
					{
						await signInManager.SignInAsync(User, isPersistent: false);
						return RedirectToAction("Index", "Home");
					}

					foreach(var err in Result.Errors)
					{
						ModelState.AddModelError(string.Empty, err.Description);
					}

				}
				catch(System.Exception e)
				{
					System.Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n");
					System.Console.WriteLine(e);
					System.Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n");
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[AcceptVerbs("GET", "POST")]
		public async Task<IActionResult> VerifyEmail(string Email)
		{
			IdentityUser User = await userManager.FindByEmailAsync(Email);
			if(User == null)
			{
				return Json(true);
			}
			return Json($"Email {Email} is already in use.");
		}

		[AcceptVerbs("GET", "POST")]
		public async Task<IActionResult> VerifyUsername(string Username)
		{
			IdentityUser User = await userManager.FindByNameAsync(Username);

			if(User == null)
			{
				return Json(true);
			}
			return Json($"{Username} is already in use.");
		}
	}
}