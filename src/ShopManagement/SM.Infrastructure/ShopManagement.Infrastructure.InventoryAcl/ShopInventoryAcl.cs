using InventoryManagement.Application.Contracts.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public async Task<bool> ReduceFromInventory(List<OrderItem> items)
        {
            var command = items.Select(orderItem =>
                    new DecreaseInventory(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId))
                .ToList();

            var isSucceeded =  await _inventoryApplication.Reduce(command);
            return isSucceeded.IsSucceeded;

        }
    }
    
}
