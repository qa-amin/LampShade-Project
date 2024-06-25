using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
	{
		private readonly IProductCategoryRepository _productCategoryRepository;

        private readonly IFileUploader _fileUploader;

		public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository, IFileUploader fileUploader)
		{
			_productCategoryRepository = productCategoryRepository;
            _fileUploader = fileUploader;
		}

		public async Task<OperationResult> Create(CreateProductCategory command)
		{
			var opration = new OperationResult();
			if (await _productCategoryRepository.Exists(x => x.Name == command.Name))
			{
				return opration.Failed(ApplicationMessages.DuplicatedRecord);
			}

			var Slug = command.Slug.Slugify();

            var picturePath = $"{command.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, picturePath);
            ProductCategory productCategory = new ProductCategory(command.Name, command.Description,
                pictureName, command.PictureAlt,
				command.PictureTitle, command.KeyWords, command.MetaDescription, Slug);

			await _productCategoryRepository.Create(productCategory);
			await _productCategoryRepository.SaveChanges();

			return  opration.Succeeded();
		}



		public async Task<OperationResult> Edit(EditProductCategory command)
		{
			var operation = new OperationResult();

			var productCategory = await _productCategoryRepository.Get(command.Id);
			if (productCategory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

			if (await _productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
			{
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }


			var Slug = command.Slug.Slugify();
            var picturePath = $"{command.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, picturePath);
			productCategory.Edit(command.Name, command.Description,
                pictureName, command.PictureAlt,
				command.PictureTitle, command.KeyWords, command.MetaDescription, Slug);

			await _productCategoryRepository.SaveChanges();
			return operation.Succeeded();
		}


      

        public async Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel)
        {
			return await _productCategoryRepository.Search(searchModel.Name);
        }

        public async Task<EditProductCategory> GetDetails(long Id)
        {
            var productCategory = await  _productCategoryRepository.GetDetails(Id);

            return productCategory;

        }

        public async Task<List<ProductCategoryViewModel>> GetProductCategories()
        {
            var listProductCategories = await _productCategoryRepository.GetProductCategories();
            return listProductCategories;
        }


	}
}