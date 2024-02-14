using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class ColleagueDiscountRepository : RepositoryBase<long, ColleagueDiscount>, IColleagueDiscountRepository
    {
        private readonly DiscountManagementDbContext _context;
        private readonly ShopManagementDbContext _shopContext;

        public ColleagueDiscountRepository(DiscountManagementDbContext context, ShopManagementDbContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public async Task<List<ColleagueDiscountViewModel>> Search(long id)
        {
            var products = await _shopContext.Products.Select(x => new { x.Id, x.Name }).ToListAsync();
            var query = await _context.ColleagueDiscounts.Select(x => new ColleagueDiscountViewModel
            {
                Id = x.Id,
                CreationDate = x.CreationDate.ToFarsi(),
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId,
                IsRemoved = x.IsRemoved
            }).ToListAsync();

            if (id > 0)
                query = query.Where(x => x.ProductId == id).ToList();

            var discounts = query.OrderByDescending(x => x.Id).ToList();
            discounts.ForEach(discount =>
                discount.Product = products.FirstOrDefault(x => x.Id == discount.ProductId)?.Name);
            return discounts;
        }

        public async Task<EditColleagueDiscount> GetDetails(long id)
        {
            return await _context.ColleagueDiscounts.Select(x => new EditColleagueDiscount
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId
            }).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
