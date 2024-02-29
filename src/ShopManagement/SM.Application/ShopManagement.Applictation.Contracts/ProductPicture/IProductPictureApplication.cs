using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.ProductPicture
{
    public interface IProductPictureApplication
    {
        Task<OperationResult> Create(CreateProductPicture command);
        Task<OperationResult> Edit(EditProductPicture command);
        Task<OperationResult> Remove(long id);
        Task<OperationResult> Restore(long id);
        Task<EditProductPicture> GetDetails(long id);
        Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel  searchModel);
    }
}
