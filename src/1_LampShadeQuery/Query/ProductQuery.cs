﻿using _0_Framework.Application;
using _1_LampShadeQuery.Contracts.Comment;
using _1_LampShadeQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _1_LampShadeQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopManagementDbContext _shopManagementDbContext;
        private readonly InventoryManagementDbContext _inventoryManagementDbContext;
        private readonly DiscountManagementDbContext _discountManagementDbContext;

        private readonly CommentManagementDbContext _commentManagementDbContext;

        public ProductQuery(ShopManagementDbContext shopManagementDbContext, InventoryManagementDbContext inventoryManagementDbContext, DiscountManagementDbContext discountManagementDbContext, CommentManagementDbContext commentManagementDbContext)
        {
            _shopManagementDbContext = shopManagementDbContext;
            _inventoryManagementDbContext = inventoryManagementDbContext;
            _discountManagementDbContext = discountManagementDbContext;
            _commentManagementDbContext = commentManagementDbContext;
        }

        public async Task<List<ProductQueryModel>> GetLatestArrivals()
        {
	        var products = _shopManagementDbContext.Products
		        .Include(x => x.Category);


	        var inventories =
		         _inventoryManagementDbContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice });

	        var discounts =  _discountManagementDbContext.CustomerDiscounts
		        .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
		        .Select(x => new { x.ProductId, x.DiscountRate });

            var productQueryModel = await products.Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                Category = x.Category.Name,
                
            }).OrderByDescending(x => x.Id).Take(6).ToListAsync();

            foreach (var product in productQueryModel)
            {
                var inventory = await inventories.FirstOrDefaultAsync(x => x.ProductId == product.Id);
                if (inventory != null)
                {
                    var price = inventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = await discounts.FirstOrDefaultAsync(x => x.ProductId == product.Id);
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


            return productQueryModel;

        }

        public async Task<List<ProductQueryModel>> Search(string value)
        {
	        var inventories =
		         _inventoryManagementDbContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice });

	        var discounts =  _discountManagementDbContext.CustomerDiscounts
		        .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
		        .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate });


            var query =  _shopManagementDbContext.Products.Include(x => x.Category)

                .Select(x => new ProductQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    ShortDescription = x.ShortDescription



                }).AsNoTracking();


            if (!string.IsNullOrWhiteSpace(value))
            {
	            query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));
            }

            var products = await query.OrderByDescending(x => x.Id).ToListAsync();


            foreach (var product in products)
            {
                var inventory = await inventories.FirstOrDefaultAsync(x => x.ProductId == product.Id);
                if (inventory != null)
                {
                    var price = inventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = await discounts.FirstOrDefaultAsync(x => x.ProductId == product.Id);
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


            return products;
        }

        public async Task<ProductQueryModel> GetProductDetails(string slug)
        {
	        var products = _shopManagementDbContext.Products
		        .Include(x => x.Category)
		        .Include(x => x.ProductPictures);





	        var inventories =
		        _inventoryManagementDbContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice, x.InStock });

	        var discounts =  _discountManagementDbContext.CustomerDiscounts
		        .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
		        .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate });

            var product = await products.Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                Category = x.Category.Name,
                CategorySlug = x.Category.Slug,
                Code = x.Code,
                Description = x.Description,
                Keywords = x.KeyWords,
                MetaDescription = x.MetaDescription,
                ShortDescription = x.ShortDescription,
                Pictures = MapProductPictures(x.ProductPictures),
                

            }).FirstOrDefaultAsync(x => x.Slug == slug);

            if (product == null)
                return new ProductQueryModel();

            
            
                var inventory = await inventories.FirstOrDefaultAsync(x => x.ProductId == product.Id);
                if (inventory != null)
                {
                    product.IsInStock = inventory.InStock;
                    var price = inventory.UnitPrice;
                    product.Price = price.ToMoney();
                    product.DoublePrice = price;
                    var discount = await discounts.FirstOrDefaultAsync(x => x.ProductId == product.Id);
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
            product.Comments = await _commentManagementDbContext.Comments
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x => x.Type == CommentType.Product)
                .Where(x => x.OwnerRecordId == product.Id)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Message = x.Message,
                    Name = x.Name,
                    CreationDate = x.CreationDate.ToFarsi()
                }).OrderByDescending(x => x.Id).ToListAsync();
            return  product;

        }

        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
        {
            return pictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemove,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemoved).ToList();
        }

        public async Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems)
        {
            if (cartItems == null)
                return new List<CartItem>();

            var inventory = await _inventoryManagementDbContext.Inventory.ToListAsync();

            foreach (var cartItem in cartItems.Where(cartItem =>
                         inventory.Any(x => x.ProductId == cartItem.Id && x.InStock)))
            {
                var itemInventory = inventory.Find(x => x.ProductId == cartItem.Id);
                cartItem.IsInStock = itemInventory.CalculateCurrentCount() >= cartItem.Count;
            }

            return cartItems;
        }
    }
}
