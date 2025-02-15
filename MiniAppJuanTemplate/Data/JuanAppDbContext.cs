using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Data
{
	public class JuanAppDbContext : IdentityDbContext<AppUser>
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
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<SubscribeEmail> SubscribeEmails { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseAuditableEntity>();
            foreach (var entire in entries)
            {
                if (entire.State == EntityState.Added)
                {
                    entire.Property(p => p.CreatedDate).CurrentValue = DateTime.Now;
                }
                if (entire.State == EntityState.Modified)
                {
                    entire.Property(p => p.UpdatedDate).CurrentValue = DateTime.Now;
                }
                if (entire.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entire.Property(p => p.DeletedDate).CurrentValue = DateTime.Now;
                }
                if (entire.State == EntityState.Deleted)
                {
                    entire.Property(p => p.DeletedDate).CurrentValue = DateTime.Now;
                    entire.Property(p => p.IsDeleted).CurrentValue = true;
                    entire.State = EntityState.Modified;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
