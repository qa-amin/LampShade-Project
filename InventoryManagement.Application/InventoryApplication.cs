using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Domain.ProductAgg;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public InventoryApplication(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public OperationResult Create(CreateInventory command)
        {
            var operationResult = new OperationResult();
            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord); 
            }
            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            _inventoryRepository.Create(inventory);
            _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public OperationResult Edit(EditInventory command)
        {
            var operationResult = new OperationResult();

            var editInventory = _inventoryRepository.GetDetails(command.Id);
            if (editInventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id ))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }
            editInventory.Edit(command.ProductId, command.UnitPrice);
            
            _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public EditInventory GetDetails(long id)
        {
            var inventory = _inventoryRepository.GetDetails(id);

            var editInventory = new EditInventory
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                UnitPrice = inventory.UnitPrice
            };

            return editInventory;
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _productRepository.GetProducts().Select(x => new { x.Id, x.Name }).ToList();
            var inventoryViewModelList = _inventoryRepository.Search(searchModel.ProductId, searchModel.InStock).Select(x => new InventoryViewModel
            {
                UnitPrice = x.UnitPrice,
                Id = x.Id,
                ProductId = x.ProductId,
                InStock = x.InStock,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi()
            }).ToList();

            inventoryViewModelList.ForEach(item =>
                item.Product = products.FirstOrDefault(x => x.Id == item.Id)?.Name);


            return inventoryViewModelList;
        }

        
        public OperationResult Increase(IncreaseInventory command)
        {
            var operationResult = new OperationResult();
            var inventory = _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            long operatorId = 1;
            inventory.Increase(command.Count,operatorId, command.Description);
            _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public OperationResult Reduce(List<DecreaseInventory> command)
        {
            var operationResult = new OperationResult();
            long operatorId = 1;
            foreach (var item in command)
            {
                var inventory = _inventoryRepository.GetBy(item.ProductId);
                inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
            }
            _inventoryRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Reduce(DecreaseInventory command)
        {
            var operationResult = new OperationResult();
            var inventory = _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            long operatorId = 1;
            inventory.Reduce(command.Count, operatorId, command.Description,0);
            _inventoryRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            var inventoryOperations = _inventoryRepository.GetOperationLog(inventoryId);
            var inventoryOperationViewModel = inventoryOperations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Description = x.Description,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                Operator = "مدیر سیستم",
                OperatorId = x.OperatorId,
                OrderId = x.OrderId

            }).OrderByDescending(x => x.Id).ToList();

            return inventoryOperationViewModel;
        }
    }
}