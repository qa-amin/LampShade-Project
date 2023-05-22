using _0_Framework.Domain;

namespace ShopManagement.Domain.ProductCategoryAgg
{
	public interface IProductCategoryRepository : IRepository<long, ProductCategory>
	{


        List<ProductCategory> GetProductCategories();


        ProductCategory GetDetails(long id);
		
		List<ProductCategory> Search(string searchModel);
	}
}
