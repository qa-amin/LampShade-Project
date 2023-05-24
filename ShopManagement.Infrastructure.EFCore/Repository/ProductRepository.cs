using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long, Product> , IProductRepository
    {
        private readonly ShopManagementDbContext _context;

        public ProductRepository( ShopManagementDbContext context) : base(context)
        {
            _context = context;
        }


        public List<Product> Search(string? Name, string? Code, long? CategoryId)
        {
            var query = _context.Products.Include(x => x.Category).ToList();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x => x.Name.Contains(Name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(Code))
            {
                query = query.Where(x => x.Code.Contains(Code)).ToList();
            }

            if (CategoryId != 0 && CategoryId != null)
            {
                query = query.Where(x => x.CategoryId == CategoryId).ToList();
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public Product GetDetails(long Id)
        {
            return _context.Products.Find(Id);
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductWithCategory(long id)
        {
            return _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }
    }
}
