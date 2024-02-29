using System;
using System.Globalization;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;


namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository : RepositoryBase<long , DiscountManagement.Domain.CustomerDiscountAgg.CustomerDiscount> , ICustomerDiscountRepository
    {
        private readonly DiscountManagementDbContext _context;
        private readonly ShopManagementDbContext _shopContext;

        public CustomerDiscountRepository(DiscountManagementDbContext context, ShopManagementDbContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }


        public async Task<List<CustomerDiscountViewModel>> search(long? productId, string? startDate, string? endDate)
        {

            var products = await _shopContext.Products.Select(x => new { x.Id, x.Name }).ToListAsync();
            var query = _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                EndDate = x.EndDate.ToFarsi(),
                EndDateGr = x.EndDate,
                StartDate = x.StartDate.ToFarsi(),
                StartDateGr = x.StartDate,
                ProductId = x.ProductId,
                Reason = x.Reason,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (productId > 0)
                query = query.Where(x => x.ProductId == productId);

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                query = query.Where(x => x.StartDateGr > startDate.ToGeorgianDateTime());
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                query = query.Where(x => x.EndDateGr < endDate.ToGeorgianDateTime());
            }

            var discounts = query.OrderByDescending(x => x.Id).ToList();

            discounts.ForEach(discount =>
                discount.Product = products.FirstOrDefault(x => x.Id == discount.ProductId)?.Name);

            return discounts;
        }

        public async Task<EditCustomerDiscount> GetDetails(long id)
        {
            return await _context.CustomerDiscounts.Select(x => new EditCustomerDiscount()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                DiscountRate = x.DiscountRate,
                StartDate = x.StartDate.ToString(CultureInfo.InvariantCulture),
                EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
                Reason = x.Reason
            }).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
