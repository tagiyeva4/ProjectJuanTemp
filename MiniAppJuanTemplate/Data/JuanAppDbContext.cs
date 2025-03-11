using Microsoft.AspNetCore.Identity;
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
       
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
            modelBuilder.Entity<Tag>().HasQueryFilter(x => !x.IsDeleted);



           // #region seedData

           // var userId = Guid.NewGuid().ToString();
           // var adminId = Guid.NewGuid().ToString();
           // var memberId = Guid.NewGuid().ToString();
           // var superAdminId = Guid.NewGuid().ToString();


           // //Seeding a  'Administrator' role to AspNetRoles table
           // modelBuilder.Entity<IdentityRole>().HasData(
           //     new IdentityRole
           //     {
           //         Id = adminId,
           //         Name = "Admin",
           //         NormalizedName = "ADMIN".ToUpper()
           //     },
           //     new IdentityRole
           //     {
           //         Id = memberId,
           //         Name = "Member",
           //         NormalizedName = "MEMBER".ToUpper()
           //     },
           //     new IdentityRole
           //     {
           //         Id = superAdminId,
           //         Name = "SuperAdmin",
           //         NormalizedName = "SUPERADMIN".ToUpper()
           //     }
           // );
           // //a hasher to hash the password before seeding the user to the db
           // var hasher = new PasswordHasher<IdentityUser>();

           // modelBuilder.Entity<AppUser>().HasData(
           //new AppUser
           //{
           //    Id = userId, // primary key
           //    FullName = "Test",
           //    UserName = "_test",
           //    NormalizedUserName = "_Test",
           //    PasswordHash = hasher.HashPassword(null, "12345@Tt")
           //}
           // );
           // //Seeding the relation between our user and role to AspNetUserRoles table
           // modelBuilder.Entity<IdentityUserRole<string>>().HasData(
           //     new IdentityUserRole<string>
           //     {
           //         RoleId = memberId,
           //         UserId = userId
           //     }
           // );

           // #endregion
        }
    }
}
