using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using InventoryManagement.Application.Contracts.Inventory;

namespace InventoryManagement.Domain.InventoryAgg
{
    public interface IInventoryRepository : IRepository<long , Inventory>
    {
        Task<EditInventory> GetDetails(long id);
        Task<List<InventoryViewModel>> Search(long? productId, bool? inStock);
        Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId);
    }
}
