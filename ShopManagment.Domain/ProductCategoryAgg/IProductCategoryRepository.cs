using _0_Framework.Domain;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg
{
	public interface IProductCategoryRepository : IRepository<long, ProductCategory>
	{


        Task<List<ProductCategoryViewModel>> GetProductCategories();

        Task<EditProductCategory> GetDetails(long id);
		
		Task<ProductCategoryViewModel> Search(string searchModel);

		Task<string> GetSlugById(long id);
	}
}
