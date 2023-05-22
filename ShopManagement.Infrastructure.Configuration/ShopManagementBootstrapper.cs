﻿using _1_LampShadeQuery.Contracts.Product;
using _1_LampShadeQuery.Contracts.ProductCategory;
using _1_LampShadeQuery.Contracts.Slide;
using _1_LampShadeQuery.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;


namespace ShopManagement.Infrastructure.Configuration
{
	public class ShopManagementBootstrapper
	{
		public static void Config(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShopManagementDbContext>(options => options.UseSqlServer(connectionString));


            services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
			services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
			


			services.AddTransient<IProductApplication, ProductApplication>();
			services.AddTransient<IProductRepository, ProductRepository>();

            services.AddTransient<IProductPictureApplication, ProductPictureApplication>();
            services.AddTransient<IProductPictureRepository, ProductPictureRepository>();

            services.AddTransient<ISlideApplication, SlideApplication>();
            services.AddTransient<ISlideRepository, SlideRepository>();

            services.AddTransient<ISlideQuery, SlideQuery>();
            services.AddTransient<IProductQuery, ProductQuery>();

            services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();

        }
	}
}