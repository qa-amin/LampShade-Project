using Microsoft.EntityFrameworkCore;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
	public class ProductCategoryRepository : RepositoryBase<long, ProductCategory> , IProductCategoryRepository
	{
		private readonly ShopManagementDbContext _context;

		public ProductCategoryRepository(ShopManagementDbContext context):base(context) 
		{
			_context = context;
		}


        public async Task<List<ProductCategoryViewModel>> GetProductCategories()
        {
            return await _context.ProductCategories
                .Select(pc => new ProductCategoryViewModel()
            {
                    CreationDate = pc.CreationDate.ToFarsi(),
                    Id = pc.Id,
                    Name = pc.Name,
                    Picture = pc.Picture
            }).ToListAsync();
        }

        public async Task<EditProductCategory> GetDetails(long id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            return new EditProductCategory()
            {
                Description = productCategory.Description,
                Id = productCategory.Id,
                KeyWords = productCategory.KeyWords,
                MetaDescription = productCategory.MetaDescription,
                Name = productCategory.MetaDescription,
                PictureAlt = productCategory.PictureAlt,
                PictureTitle = productCategory.PictureTitle,
                Slug = productCategory.Slug
            };
        }

        public async Task<List<ProductCategoryViewModel>> Search(string searchModel)
        {
            var query = await _context.ProductCategories.ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchModel))
            {
               query = query.Where(x => x.Name.Contains(searchModel)).OrderByDescending(x => x.Id).ToList();
            }

			return  query.OrderByDescending(x => x.Id).Select(pc => new ProductCategoryViewModel()
            {
                CreationDate = pc.CreationDate.ToFarsi(),
                Id = pc.Id,
                Name = pc.Name,
                Picture = pc.Picture
            }).ToList();
        }

        public async Task<string> GetSlugById(long id)
        {
            var getBySlug =  await _context.ProductCategories.Select(x => new { x.Slug, x.Id }).FirstOrDefaultAsync(x => x.Id == id);
            return getBySlug.Slug;
        }
    }
}
