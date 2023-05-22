using System.Security.Cryptography.X509Certificates;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
	{
		private readonly IProductCategoryRepository _productCategoryRepository;

		public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
		{
			_productCategoryRepository = productCategoryRepository;
		}

		public OperationResult Create(CreateProductCategory command)
		{
			var opration = new OperationResult();
			if (_productCategoryRepository.Exists(x => x.Name == command.Name))
			{
				return opration.Failed(ApplicationMessages.DuplicatedRecord);
			}

			var Slug = command.Slug.Slugify();


			ProductCategory productCategory = new ProductCategory(command.Name, command.Description,
				command.Picture, command.PictureAlt,
				command.PictureTitle, command.KeyWords, command.MetaDescription, Slug);

			_productCategoryRepository.Create(productCategory);
			_productCategoryRepository.SaveChanges();

			return opration.Succeeded();
		}



		public OperationResult Edit(EditProductCategory command)
		{
			var operation = new OperationResult();

			var productCategory = _productCategoryRepository.Get(command.Id);
			if (productCategory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

			if (_productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
			{
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }


			var Slug = command.Slug.Slugify();
			productCategory.Edit(command.Name, command.Description,
				command.Picture, command.PictureAlt,
				command.PictureTitle, command.KeyWords, command.MetaDescription, Slug);

			_productCategoryRepository.SaveChanges();
			return operation.Succeeded();
		}

        public ProductCategoryViewModel Get(long Id)
        {
            var productCategory = _productCategoryRepository.Get(Id);

            var productCategoryViewModel = new ProductCategoryViewModel
            {
                Name = productCategory.Name,
                CreationDate = productCategory.CreationDate.ToFarsi(),
                Picture = productCategory.Picture,
                Id = productCategory.Id,
                ProductsCount = 0
            };
			
			return productCategoryViewModel;
        }

        public List<ProductCategoryViewModel> Get()
        {
            List<ProductCategory> productCategory = _productCategoryRepository.Get();
            List<ProductCategoryViewModel> productCategoryVeiwModels = new List<ProductCategoryViewModel>();
            foreach (var item in productCategory)
            {
                productCategoryVeiwModels.Add(new ProductCategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationDate = item.CreationDate.ToFarsi(),
                    Picture = item.Picture,
                    ProductsCount = 0
                });
            }
            return productCategoryVeiwModels;
        }

      

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            List<ProductCategory> productCategory = _productCategoryRepository.Search(searchModel.Name);
     //       List<ProductCategoryViewModel> productCategoryVeiwModels = new List<ProductCategoryViewModel>();
     //       foreach (var item in productCategory)
     //       {
     //           productCategoryVeiwModels.Add(new ProductCategoryViewModel
     //           {
					//Id = item.Id,
					//Name = item.Name,
					//CreationDate = item.CreationDate.ToString(),
					//Picture = item.Picture,
					//ProductsCount = 0
     //           });
     //       }

     var productCategoryVeiwModels = _productCategoryRepository.Search(searchModel.Name).Select(x =>
         new ProductCategoryViewModel
         {
			 Id = x.Id,
			 Name = x.Name,
			 CreationDate = x.CreationDate.ToFarsi(),
			 Picture = x.Picture,
			 ProductsCount = 0

		 }).ToList();
			return productCategoryVeiwModels;
        }

        public EditProductCategory GetDetails(long Id)
        {
            var productCategory = _productCategoryRepository.GetDetails(Id);

            var editProductCategory = new EditProductCategory
            {
                Name = productCategory.Name,
                Description = productCategory.Description,
                PictureTitle = productCategory.PictureTitle,
                Picture = productCategory.Picture,
                PictureAlt = productCategory.PictureAlt,
                KeyWords = productCategory.KeyWords,
                MetaDescription = productCategory.MetaDescription,
                Slug = productCategory.Slug,
                Id = productCategory.Id
            };

            return editProductCategory;

        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            var listProductCategories = _productCategoryRepository.GetProductCategories();
            var listProductCategoriesViewModel = listProductCategories.Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return listProductCategoriesViewModel;
        }

        //public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
		//{
		//	return _productCategoryRepository.Search(searchModel);
		//}
		//public ProductCategory GetDetails(long Id)
		//{
		//	return _productCategoryRepository.GetDetails(Id);
		//}

	}
}