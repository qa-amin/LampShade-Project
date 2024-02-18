using System.Data.Entity;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
    {
        private readonly ShopManagementDbContext _context;
        private readonly AccountManagementDbContext _accountContext;

        public OrderRepository(ShopManagementDbContext context, AccountManagementDbContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
        }

        public async Task<double> GetAmountBy(long id)
        {
            var order = await _context.Orders
                .Select(x => new {x.PayAmount, x.Id})
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order != null)
                return order.PayAmount;
            return 0;
        }

        public async Task<List<OrderItemViewModel>> GetItems(long orderId)
        {

            var products = _context.Products.Select(x => new { x.Id, x.Name });
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
                return new List<OrderItemViewModel>();

            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            });

            foreach (var item in items)
            {
                var product = await products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                item.Product = product.Name;
            }

            return  items.ToList();
        }


        public async Task<List<OrderViewModel>> Search(long ?accountId, bool? isCanceled)
        {
            var accounts =  _accountContext.Accounts.Select(x => new { x.Id, x.Fullname });
            var query = _context.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                DiscountAmount = x.DiscountAmount,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (isCanceled != null) query = query.Where(x => x.IsCanceled == isCanceled);

            if (accountId > 0) query = query.Where(x => x.AccountId == accountId);

            var orders = query.OrderByDescending(x => x.Id);
            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.Fullname;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
            }

            return await orders.ToListAsync();
        }
    }
}