using System.Threading.Tasks;
using Mangary.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mangary
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost Host = CreateHostBuilder(args).Build();

			using(IServiceScope Scope = Host.Services.CreateScope())
			{
				RoleManager<IdentityRole> roleManager = Scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

				await DbSeeder.SeedDataAsync(roleManager);
			}

			await Host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
