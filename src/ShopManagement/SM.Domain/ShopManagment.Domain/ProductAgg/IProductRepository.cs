using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Domain.ProductAgg
{
    public interface IProductRepository : IRepository<long, Product>
    {
        Task<List<ProductViewModel>> Search(string? name, string? code, long? categoryId);
        Task<EditProduct> GetDetails(long id);
        Task<List<ProductViewModel>> GetProducts();
        Task<ProductViewModel> GetProductWithCategory(long id);
    }
}
