using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;

namespace ShopManagement.Domain.ProductAgg
{
    public interface IProductRepository : IRepository<long, Product>
    {
        List<Product> Search(string? Name, string? Code, long? CategoryId);
        

        Product GetDetails(long Id);

        List<Product> GetProducts();
    }
}
