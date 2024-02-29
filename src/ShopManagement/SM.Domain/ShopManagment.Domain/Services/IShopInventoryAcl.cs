using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Domain.Services
{
    public interface IShopInventoryAcl
    {
        Task<bool> ReduceFromInventory(List<OrderItem> items);
    }
}