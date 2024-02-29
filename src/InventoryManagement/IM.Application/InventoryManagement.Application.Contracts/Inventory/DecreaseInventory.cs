using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Contracts.Inventory
{
    public class DecreaseInventory
    {
        public long ProductId { get; set; }
        public long InventoryId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
        public long OrderId { get; set; }

        public DecreaseInventory()
        {

        }

        public DecreaseInventory(long productId, long count, string description, long orderId)
        {
            ProductId = productId;
            Count = count;
            Description = description;
            OrderId = orderId;
        }
    }
}
