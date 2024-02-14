using _0_Framework.Domain;

namespace ShopManagement.Domain.ProductPictureAgg
{
    public interface IProductPictureRepository : IRepository<long, ProductPicture>
    {
        Task<ProductPicture> GetDetails(long id);
        Task<List<ProductPicture>> search(long? productId);
        

    }
}
