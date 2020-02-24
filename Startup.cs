using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mangary.Data;
using Microsoft.AspNetCore.Identity;

namespace Mangary
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("MangaryContext")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
			})
                .AddEntityFrameworkStores<AppDbContext>();

			services.AddAuthentication();
			services.AddAuthorization();


			services.ConfigureApplicationCookie(options => {
				options.LoginPath = "/Account/SignIn";
//				options.AccessDeniedPath = "/404";
			});

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
				app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
			/* 
			app.Use(async (context, next) => {
				await next();
				if(context.Response.StatusCode == 404)
				{
					context.Request.Path = "/";
					await next();
				}
			});
*/
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


			app.UseAuthentication();
			app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
