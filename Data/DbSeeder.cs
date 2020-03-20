using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Mangary.Data
{
	public static class DbSeeder
	{
		public static async Task SeedDataAsync(RoleManager<IdentityRole> roleManager)
		{
			System.Boolean ManagerExists = await roleManager.RoleExistsAsync("Manager");

			if(!ManagerExists)
			{
				IdentityRole Manager = new IdentityRole("Manager");

				IdentityResult Result = await roleManager.CreateAsync(Manager);

				if(Result.Succeeded)
				{
					System.Console.Write("\n\n\n\n\n\n\n");
					System.Console.Write("Manager role has been created successfully.");
					System.Console.Write("\n\n\n\n\n\n\n");
				}
			}
		}
	}
}