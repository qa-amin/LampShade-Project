using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;


namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IAuthHelper _authHelper;

        public InventoryApplication(IInventoryRepository inventoryRepository, IAuthHelper authHelper)
        {
            _inventoryRepository = inventoryRepository;
            _authHelper = authHelper;
        }

        public async Task<OperationResult> Create(CreateInventory command)
        {
            var operationResult = new OperationResult();
            if (await _inventoryRepository.Exists(x => x.ProductId == command.ProductId))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord); 
            }
            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            await _inventoryRepository.Create(inventory);
            await _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Edit(EditInventory command)
        {
            var operationResult = new OperationResult();

            var editInventory = await _inventoryRepository.Get(command.Id);
            if (editInventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id ))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }
            editInventory.Edit(command.ProductId, command.UnitPrice);
            
            await _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public async Task<EditInventory> GetDetails(long id)
        {
            return await _inventoryRepository.GetDetails(id);
        }

        public Task<List<InventoryViewModel>> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel.ProductId, searchModel.InStock);
        }

        
        public async Task<OperationResult> Increase(IncreaseInventory command)
        {
            var operationResult = new OperationResult();
            var inventory = await _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            long operatorId = 1;
            inventory.Increase(command.Count,operatorId, command.Description);
            await _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Reduce(List<DecreaseInventory> command)
        {
            var operationResult = new OperationResult();
            long operatorId =  _authHelper.CurrentAccountId();
            foreach (var item in command)
            {
                var inventory = await _inventoryRepository.Get(item.ProductId);
                inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
            }
            await _inventoryRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Reduce(DecreaseInventory command)
        {
            var operationResult = new OperationResult();
            var inventory = await _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            long operatorId = _authHelper.CurrentAccountId();
            inventory.Reduce(command.Count, operatorId, command.Description,0);
            await _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public async Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId)
        {
            return await _inventoryRepository.GetOperationLog(inventoryId);
        }
    }
}