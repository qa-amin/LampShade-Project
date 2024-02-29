using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopManagement.Application.Contracts.Order;

namespace _1_LampShadeQuery.Contracts.Product
{
    public interface IProductQuery
    {
        Task<List<ProductQueryModel>> GetLatestArrivals();

        Task<List<ProductQueryModel>> Search(string value);

        Task<ProductQueryModel> GetProductDetails(string slug);
        Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems);
    }
}
