using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.Product
{
    public interface IProductApplication
    {
        Task<OperationResult> Create(CreateProduct command);
        Task<OperationResult> Edit(EditProduct command);
        Task<EditProduct> GetDetails(long id);
        Task<List<ProductViewModel>> Search(ProductSearchModel searchModel);
        Task<List<ProductViewModel>> GetProducts();
    }
}
