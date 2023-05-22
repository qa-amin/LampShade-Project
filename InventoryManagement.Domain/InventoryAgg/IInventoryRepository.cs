using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;

namespace InventoryManagement.Domain.InventoryAgg
{
    public interface IInventoryRepository : IRepository<long , Inventory>
    {
        Inventory GetDetails(long id);
        List<Inventory> Search(long? productId, bool? inStock);
        Inventory GetBy(long productId);
        List<InventoryOperation> GetOperationLog(long inventoryId);
    }
}
