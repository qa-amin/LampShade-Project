using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory
{
    public interface IInventoryApplication
    {
        Task<OperationResult> Create(CreateInventory command);
        Task<OperationResult> Edit(EditInventory command);
        Task<OperationResult> Increase(IncreaseInventory command);
        Task<OperationResult> Reduce(List<DecreaseInventory> command);
        Task<OperationResult> Reduce(DecreaseInventory command);
        Task<EditInventory> GetDetails(long id);
        Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel);
        
        Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId);

    }
}
