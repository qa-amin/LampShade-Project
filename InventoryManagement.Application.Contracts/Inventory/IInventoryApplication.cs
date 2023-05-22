using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory
{
    public interface IInventoryApplication
    {
        OperationResult Create(CreateInventory command);
        OperationResult Edit(EditInventory command); 
        EditInventory GetDetails(long id);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
        OperationResult Increase(IncreaseInventory command);
        OperationResult Reduce(List<DecreaseInventory> command);
        OperationResult Reduce(DecreaseInventory command);
        List<InventoryOperationViewModel> GetOperationLog(long inventoryId);

    }
}
