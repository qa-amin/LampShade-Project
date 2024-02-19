using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _1_LampShadeQuery.Contracts.Product;
using _1_LampShadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _1_LampShadeQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopManagementDbContext _shopManagementDbContext;
        private readonly InventoryManagementDbContext _inventoryManagementDbContext;
        private readonly DiscountManagementDbContext _discountManagementDbContext;

        public ProductCategoryQuery(ShopManagementDbContext shopManagementDbContext, InventoryManagementDbContext inventoryManagementDbContext, DiscountManagementDbContext discountManagementDbContext)
        {
            _shopManagementDbContext = shopManagementDbContext;
            _inventoryManagementDbContext = inventoryManagementDbContext;
            _discountManagementDbContext = discountManagementDbContext;
        }

        public async Task<List<ProductCategoryQueryModel>> GetProductCategories()
        {
            return await _shopManagementDbContext.ProductCategories.Select(x => new ProductCategoryQueryModel
            {
                Name = x.Name,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                Id = x.Id
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductCategoryQueryModel>> GetProductCategoriesWithProducts()
        {
            var inventories = await _inventoryManagementDbContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToListAsync();

            var discounts = await _discountManagementDbContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new {x.ProductId, x.DiscountRate }).ToListAsync();


	        var categories = await _shopManagementDbContext.ProductCategories.Include(x => x.Products)
		        .ThenInclude(x => x.Category)
		        .Select(x => new ProductCategoryQueryModel
		        {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Products = MapProducts(x.Products)
                    
                    

                }).AsNoTracking().ToListAsync();

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    var inventory = inventories.FirstOrDefault(x => x.ProductId == product.Id);
                    if (inventory != null)
                    {
                        var price = inventory.UnitPrice;
                        product.Price = price.ToMoney();
                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            int discountRate = discount.DiscountRate;

                            product.DiscountRate = (int)discountRate;

                            product.HasDiscount = discountRate > 0;

                            var discountAmount = Math.Round((price * discountRate) / 100);

                            product.PriceWithDiscount = (price - discountAmount).ToMoney();
                        }
                    }
                    

                  
                   
                }
            }

            return categories;
        }

        public async Task<ProductCategoryQueryModel> GetProductCategoryWithProductsBy(string slug)
        {
            var inventories = await _inventoryManagementDbContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToListAsync();

            var discounts = await _discountManagementDbContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate , x.EndDate}).ToListAsync();


            var category = await _shopManagementDbContext.ProductCategories.Include(x => x.Products)
                .ThenInclude(x => x.Category)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Products = MapProducts(x.Products),
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                    Keywords = x.Description
                    



                }).FirstOrDefaultAsync(x => x.Slug == slug);

            
            
                foreach (var product in category.Products)
                {
                    var inventory = inventories.FirstOrDefault(x => x.ProductId == product.Id);
                    if (inventory != null)
                    {
                        var price = inventory.UnitPrice;
                        product.Price = price.ToMoney();
                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            int discountRate = discount.DiscountRate;

                            product.DiscountRate = (int)discountRate;

                            product.HasDiscount = discountRate > 0;

                            var discountAmount = Math.Round((price * discountRate) / 100);

                            product.PriceWithDiscount = (price - discountAmount).ToMoney();

                            product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        }
                    }




                }
            

            return category;
        }

        private static  List<ProductQueryModel> MapProducts(List<Product> products)
        {

	        return  products.Select(x => new ProductQueryModel
	        {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                Category = x.Category.Name,
                

                
                

	        }).OrderByDescending(x => x.Id).ToList();

        }
    }
}
