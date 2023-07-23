using System.Collections.Generic;
using _0_Framework.Domain;


namespace ShopManagement.Domain.OrderAgg
{
    public interface IOrderRepository : IRepository<long, Order>
    {
        double GetAmountBy(long id);
        List<OrderItem> GetItems(long orderId);
        List<Order> Search(long? AccountId,bool? isCanceled);
    }
}