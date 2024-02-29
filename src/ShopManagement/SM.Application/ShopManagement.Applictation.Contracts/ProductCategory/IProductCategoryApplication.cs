using _0_Framework.Application;


namespace ShopManagement.Application.Contracts.ProductCategory
{
	public interface IProductCategoryApplication
	{
		Task<OperationResult> Create(CreateProductCategory command);
        Task<OperationResult> Edit(EditProductCategory command);
		Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel);
        Task<EditProductCategory> GetDetails(long id);
        Task<List<ProductCategoryViewModel>> GetProductCategories();


    }
}
