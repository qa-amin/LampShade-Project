using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Order;


namespace ShopManagement.Domain.OrderAgg
{
    public interface IOrderRepository : IRepository<long, Order>
    {
        Task<double> GetAmountBy(long id);
        Task<List<OrderItemViewModel>> GetItems(long orderId);
        Task<List<OrderViewModel>> Search(long? accountId,bool? isCanceled);
    }
}