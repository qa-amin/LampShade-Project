using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;


namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploader _fileUploader;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _fileUploader = fileUploader;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<OperationResult> Create(CreateProduct command)
        {
            var opration = new OperationResult();
            if(await _productRepository.Exists(x => x.Name==command.Name))
                return opration.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var categorySlug = await _productCategoryRepository.GetSlugById(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            var product = new Product(command.Name,  command.Code, command.ShortDescription,
                command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug,
                command.KeyWords, command.MetaDescription, command.CategoryId);
            await _productRepository.Create(product);
            await _productRepository.SaveChanges();
            return opration.Succeeded();
        }

        public async Task<OperationResult> Edit(EditProduct command)
        {
            var opration = new OperationResult();
            var getDetails = await _productRepository.Get(command.Id);
            if (getDetails == null)
            {
                return opration.Failed(ApplicationMessages.RecordNotFound);
            }
            if (await _productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return opration.Failed(ApplicationMessages.DuplicatedRecord);
            }
            var slug = command.Slug.Slugify();
            var categorySlug = await _productCategoryRepository.GetSlugById(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            getDetails.Edit(command.Name, command.Code, command.ShortDescription,
                command.Description,picturePath, command.PictureAlt, command.PictureTitle, slug,
                command.KeyWords, command.MetaDescription, command.CategoryId);
            await _productRepository.SaveChanges();

            return opration.Succeeded();
        }

        public async Task<EditProduct> GetDetails(long id)
        {
            return await _productRepository.GetDetails(id);
        }

        public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel)
        {

            return await _productRepository.Search(searchModel.Name, searchModel.Code, searchModel.CategoryId);
            
        }

      

        public async Task<List<ProductViewModel>> GetProducts()
        {

            return await _productRepository.GetProducts();
           
        }
    }
}
