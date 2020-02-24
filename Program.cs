using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mangary.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

				DbSeeder.SeedDataAsync(roleManager);
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
