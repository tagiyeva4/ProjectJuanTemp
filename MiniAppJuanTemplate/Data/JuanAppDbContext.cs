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
	}
}
