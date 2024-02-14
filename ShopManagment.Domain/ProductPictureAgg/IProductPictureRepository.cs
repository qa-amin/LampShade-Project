using _0_Framework.Domain;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ShopManagement.Domain.ProductPictureAgg
{
    public interface IProductPictureRepository : IRepository<long, ProductPicture>
    {
        Task<EditProductPicture> GetDetails(long id);
        Task<List<ProductPictureViewModel>> search(long? productId);
        

    }
}
