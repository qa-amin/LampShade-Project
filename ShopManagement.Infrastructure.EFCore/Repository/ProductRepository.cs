using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
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


        public async Task<List<ProductViewModel>> Search(string? name, string? code, long? categoryId)
        {
            var query = await _context.Products.Include(x => x.Category).ToListAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(code))
            {
                query = query.Where(x => x.Code.Contains(code)).ToList();
            }

            if (categoryId != 0 && categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId).ToList();
            }

            return query.OrderByDescending(x => x.Id).Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                CategoryId = p.CategoryId,
                Category = p.Category.Name,
                CreationDate = p.CreationDate.ToFarsi(),
                Picture = p.Picture
            }).ToList();
        }

        public async Task<EditProduct> GetDetails(long id)
        {
            var product = await _context.Products.FindAsync(id);
            return new EditProduct()
            {
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Id = product.Id,
                PictureTitle = product.PictureTitle,
                PictureAlt = product.PictureAlt,
                KeyWords = product.KeyWords,
                MetaDescription = product.MetaDescription,
                Slug = product.Slug,
                ShortDescription = product.ShortDescription
            };
        }

        public async Task<List<ProductViewModel>> GetProducts()
        {
            return await _context.Products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                CategoryId = p.CategoryId,
                Category = p.Category.Name,
                CreationDate = p.CreationDate.ToFarsi(),
                Picture = p.Picture
            }).ToListAsync();
        }
        public async Task<ProductViewModel> GetProductWithCategory(long id)
        {
            var product =  await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            return new ProductViewModel()
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Id = product.Id,
                Category = product.Category.Name,
                Picture = product.Picture,
                CreationDate = product.CreationDate.ToFarsi()
            };
        }
    }
}
