using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductPictureRepository :RepositoryBase<long, ProductPicture>, IProductPictureRepository
    {
        private readonly ShopManagementDbContext _context;
        public ProductPictureRepository(ShopManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public ProductPicture GetDetails(long id)
        {
            return _context.ProductPictures.Find(id);
        }

        public List<ProductPicture> search(long? productId)
        {
            var query = _context.ProductPictures.Include(x => x.Product).ToList();
            if (productId != 0 && productId != null)
            {
                query = query.Where(x => x.ProductId == productId).ToList();
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
