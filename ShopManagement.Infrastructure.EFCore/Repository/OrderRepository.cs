using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using AccountMangement.Infrastructure.EFCore;
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

        public double GetAmountBy(long id)
        {
            var order = _context.Orders
                .Select(x => new {x.PayAmount, x.Id})
                .FirstOrDefault(x => x.Id == id);
            if (order != null)
                return order.PayAmount;
            return 0;
        }

        public List<OrderItem> GetItems(long orderId)
        {
            
            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
                return new List<OrderItem>();

            var items = order.Items.ToList();

            return items;
        }


        public List<Order> Search(long ?AccountId, bool? isCanceled)
        {
            return _context.Orders.ToList();
        }
    }
}