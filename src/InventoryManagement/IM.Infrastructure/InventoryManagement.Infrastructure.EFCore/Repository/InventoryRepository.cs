using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;


namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<long , Inventory>, IInventoryRepository
    {
       private  readonly  InventoryManagementDbContext _context;
       private readonly ShopManagementDbContext _shopDbContext;
       private readonly AccountManagementDbContext _accountContext;


       public InventoryRepository(InventoryManagementDbContext context, ShopManagementDbContext shopDbContext, AccountManagementDbContext accountContext) : base(context)
       {
           _context = context;
           _shopDbContext = shopDbContext;
           _accountContext = accountContext;
       }

      

       public async Task<EditInventory> GetDetails(long id)
       {
           var editInventory = await _context.Inventory.FindAsync(id);
           return new EditInventory()
           {
               Id = editInventory.Id,
               ProductId = editInventory.ProductId,
               UnitPrice = editInventory.UnitPrice
           };
       }

        public async Task<List<InventoryViewModel>> Search(long? productId, bool? inStock)
        {
            var products = await _shopDbContext.Products.Select(x => new {Id =x.Id, Name = x.Name}).ToListAsync();
            var query = await _context.Inventory.ToListAsync();
            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId).ToList();
            }

            if (inStock is false)
            {
                query = query.Where(x => !x.InStock).ToList();
            }

            return query.Select(x => new InventoryViewModel()
            {
                UnitPrice = x.UnitPrice,
                Id = x.Id,
                ProductId = x.ProductId,
                InStock = x.InStock,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi(),
                Product = products.FirstOrDefault(p => p.Id == x.ProductId)?.Name
            }).OrderByDescending(x => x.Id).ToList();

             
        }

        public async Task<List<InventoryOperationViewModel>> GetOperationLog(long inventoryId)
        {
            var inventory = await _context.Inventory.FirstOrDefaultAsync(x => x.Id == inventoryId);
            var persons = await _accountContext.Accounts.Select(x => new { Id = x.Id, Operator = x.Fullname })
                .ToListAsync();

            return inventory.Operations.Select(x => new InventoryOperationViewModel()
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Description = x.Description,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                OperatorId = x.OperatorId,
                OrderId = x.OrderId,
                Operator = persons.FirstOrDefault(p => p.Id == x.OperatorId)?.Operator
            }).ToList();
        }
    }
}
