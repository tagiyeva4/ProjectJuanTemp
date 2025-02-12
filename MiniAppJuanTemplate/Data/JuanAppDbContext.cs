using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Data
{
	public class JuanAppDbContext : DbContext
	{
		public JuanAppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<HomeService> HomeServices { get; set; }
		public DbSet<Settings> Settings { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductTag> ProductTags { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<ProductSize> ProductSizes { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Size> Sizes { get; set; }

	}
}
