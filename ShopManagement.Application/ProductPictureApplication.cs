using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileUploader _fileUploader;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository, IProductRepository productRepository, IFileUploader fileUploader)
        {
            _productPictureRepository = productPictureRepository;
            _productRepository = productRepository;
            _fileUploader = fileUploader;
        }

        public async Task<OperationResult> Create(CreateProductPicture command)
        {
            var opreationResult = new OperationResult();

            var product = await _productRepository.Get(command.ProductId);

            var path = $"{product.Category.Slug}/{product.Slug}";
           var pictureName =  _fileUploader.Upload(command.Picture, path);

            var productPicture = new ProductPicture(command.ProductId, pictureName, command.PictureTitle, command.PictureAlt);

            await _productPictureRepository.Create(productPicture);

            await _productPictureRepository.SaveChanges();

            return opreationResult.Succeeded();
        }

        public async Task<OperationResult> Edit(EditProductPicture command)
        {
            var opreationResult = new OperationResult();

            var editProductPicture = await _productPictureRepository.Get(command.Id);
            if (editProductPicture == null)
            {
                return opreationResult.Failed(ApplicationMessages.RecordNotFound);
            }
            var product = await _productRepository.Get(command.ProductId);

            var path = $"{product.Category.Slug}/{product.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, path);

            editProductPicture.Edit(command.ProductId, pictureName, command.PictureAlt, command.PictureTitle);
            await _productPictureRepository.SaveChanges();

            return opreationResult.Succeeded();
        }

        public async Task<OperationResult> Remove(long id)
        {
            var operationResult = new OperationResult();

            var productPicture = await _productPictureRepository.Get(id);

            if(productPicture == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Remove();
            await _productPictureRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Restore(long id)
        {
            var operationResult = new OperationResult();

            var productPicture = await _productPictureRepository.Get(id);

            if (productPicture == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Restore();
            await _productPictureRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<EditProductPicture> GetDetails(long id)
        {
            return await _productPictureRepository.GetDetails(id);
        }

        public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel)
        {
            return await _productPictureRepository.search(searchModel.ProductId);
        }
    }

 
}
