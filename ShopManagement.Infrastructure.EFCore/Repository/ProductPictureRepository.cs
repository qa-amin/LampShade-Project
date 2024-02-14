using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
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

        public async Task<EditProductPicture> GetDetails(long id)
        {
            var productPicture = await _context.ProductPictures.FindAsync(id);
            return new EditProductPicture()
            {
                Id = productPicture.Id,
                PictureAlt = productPicture.PictureAlt,
                PictureTitle = productPicture.PictureTitle,
                ProductId = productPicture.ProductId,
            };

        }

        public async Task<List<ProductPictureViewModel>> search(long? productId)
        {
            var query = await _context.ProductPictures.Include(x => x.Product).ToListAsync();
            if (productId != 0 && productId != null)
            {
                query = query.Where(x => x.ProductId == productId).ToList();
            }

            return query.Select(x => new ProductPictureViewModel()
            {
                Id = x.Id,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToFarsi(),
                Product = x.Product.Name,
                ProductId = x.ProductId,
                IsRemoved = x.IsRemove
            }).OrderByDescending(x => x.Id).ToList();
        }
    }
}
