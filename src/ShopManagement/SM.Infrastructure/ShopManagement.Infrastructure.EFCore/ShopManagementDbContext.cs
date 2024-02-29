using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore.Mapping;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore
{
	public class ShopManagementDbContext : DbContext
	{
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductPicture> ProductPictures { get; set; }
		public DbSet<Slide> Slides { get; set; }
		public DbSet<Order> Orders { get; set; }
		public ShopManagementDbContext(DbContextOptions<ShopManagementDbContext> opOptions) : base(opOptions) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new ProductCategoryMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new ProductPictureMapping());
			modelBuilder.ApplyConfiguration(new SlideMapping());
			modelBuilder.ApplyConfiguration(new OrderMapping());

        }
	}
}