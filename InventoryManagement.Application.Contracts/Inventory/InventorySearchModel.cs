using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Contracts.Inventory
{
    public class InventorySearchModel
    {
        public long? ProductId { get; set; }
        public bool? InStock { get; set; }
    }
}
