using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
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


        public List<ProductCategory> GetProductCategories()
        {
            return _context.ProductCategories.ToList();
        }

        public ProductCategory GetDetails(long id)
        {
            return _context.ProductCategories.Find(id);
        }

        public List<ProductCategory> Search(string searchModel)
        {
            var query = _context.ProductCategories.ToList();
            if (!string.IsNullOrWhiteSpace(searchModel))
            {
               query = query.Where(x => x.Name.Contains(searchModel)).OrderByDescending(x => x.Id).ToList();
            }

			return query.OrderByDescending(x => x.Id).ToList();
        }

        public string GetSlugById(long id)
        {
            return _context.ProductCategories.Select(x => new { x.Slug, x.Id }).FirstOrDefault(x => x.Id == id).Slug;
        }
    }
}
