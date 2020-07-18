using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mangary.Models;

namespace Mangary.Data
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions options) : base(options) { }	

		protected override void OnModelCreating(ModelBuilder builder) 
			=> base.OnModelCreating(builder);

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if(options.IsConfigured) return;
			options.UseNpgsql(System.Environment.GetEnvironmentVariable("MangaryConnectionString"));
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategories> ProductCategories { get; set; }
		public DbSet<Cart> Cart { get; set; }
	}
}