using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<long , Inventory>, IInventoryRepository
    {
       private  readonly  InventoryManagementDbContext _context;
       

       public InventoryRepository(InventoryManagementDbContext context) : base(context)
       {
           _context = context;
           
       }

      

       public Inventory GetDetails(long id)
       {
           return _context.Inventory.Find(id);
       }

        public List<Inventory> Search(long? productId, bool? inStock)
        {
            
            var query = _context.Inventory.ToList();
            if (productId > 0 && productId != null)
            {
                query = query.Where(x => x.ProductId == productId).ToList();
            }

            if (inStock != null && inStock == false)
            {
                query = query.Where(x => !x.InStock).ToList();
            }

            return query.OrderByDescending(x => x.Id).ToList();

             
        }

        public Inventory GetBy(long productId)
        {
            return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
        }

        public List<InventoryOperation> GetOperationLog(long inventoryId)
        {
            var inventory = _context.Inventory.FirstOrDefault(x => x.Id == inventoryId);

            return inventory.Operations.ToList();
        }
    }
}
