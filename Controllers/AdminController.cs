using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mangary.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;

namespace Mangary.Controllers
{
	[Authorize(Roles="Admin")]
	public class AdminController : Controller
	{
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AdminController(
			RoleManager<IdentityRole> roleManager,
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager
		)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IdentityUser User = await userManager.GetUserAsync(HttpContext.User);
			bool AdminRoleExists = await roleManager.RoleExistsAsync("Admin");

			if(!AdminRoleExists && User != null)
			{
				IdentityRole AdminRole = new IdentityRole("Admin");
				IdentityResult Result = await roleManager.CreateAsync(AdminRole);

				if(Result.Succeeded)
				{
					await userManager.AddToRoleAsync(User, "Admin");
					await signInManager.SignOutAsync();
					return RedirectToAction("RoleManager", "Admin");
				}
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult RoleManager() => View();

		[HttpGet]
		public IActionResult CreateRole() => View();

		[HttpPost]
		public async Task<IActionResult> RoleManager(RoleManagerViewModel model)
		{
			if(ModelState.IsValid)
			{
				IdentityUser User = await userManager.FindByNameAsync(model.Username);
				if(User != null)
				{
					System.Console.Write("\n\n\n\n\n{0}\n\n\n\n\n\n\n", User.Id);
					return RedirectToAction("EditRoles", "Admin", new { UserId = User.Id });
				}
				ModelState.AddModelError(string.Empty, $"{model.Username} does not exist.");
			}
			return View(model);
		}

		[HttpGet("[controller]/[action]/{UserId}")]
		public async Task<IActionResult> EditRoles(string UserId)
		{
			ViewBag.UserId = UserId;
			IdentityUser User = await userManager.FindByIdAsync(UserId);

			if(User == null)
			{
				ViewBag.ErrorMessage = "USER ID NOT FOUND.";
				return View("Not found.");
			}

			List<EditRolesViewModel> model = new List<EditRolesViewModel>();

			foreach(IdentityRole role in roleManager.Roles)
			{
				EditRolesViewModel editRolesViewModel = new EditRolesViewModel
				{
					RoleName = role.Name,
				};

				if(await userManager.IsInRoleAsync(User, role.Name))
				{
					editRolesViewModel.IsSelected = true;
				}
				else
				{
					editRolesViewModel.IsSelected = false;
				}
				model.Add(editRolesViewModel);
			}
			return View(model);
		}

		[HttpPost("[controller]/[action]/{UserId}")]
		public async Task<IActionResult> EditRoles(List<EditRolesViewModel> model, string UserId)
		{
			IdentityUser User = await userManager.FindByIdAsync(UserId);

			if(User == null)
			{
				ViewBag.ErrorMessage = $"UserId with Id = {UserId} cannot be found.";
				return View("NotFound");
			}

			IList<string> Roles = await userManager.GetRolesAsync(User);
			IdentityResult Result = await userManager.RemoveFromRolesAsync(User, Roles);

			if(!Result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
				return View(model);
			}

			Result = await userManager.AddToRolesAsync(User, model.Where(x => x.IsSelected).Select(y => y.RoleName));

			if(!Result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Cannot add selected roles to user");
				return View(model);
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
