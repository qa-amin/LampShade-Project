using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public OperationResult Create(CreateProduct command)
        {
            var opration = new OperationResult();
            if(_productRepository.Exists(x => x.Name==command.Name))
                return opration.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var categorySlug = _productCategoryRepository.GetSlugById(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            var product = new Product(command.Name,  command.Code, command.ShortDescription,
                command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug,
                command.KeyWords, command.MetaDescription, command.CategoryId);
            _productRepository.Create(product);
            _productRepository.SaveChanges();
            return opration.Succeeded();
        }

        public OperationResult Edit(EditProduct command)
        {
            var opration = new OperationResult();
            var getDetails = _productRepository.GetDetails(command.Id);
            if (getDetails == null)
            {
                return opration.Failed(ApplicationMessages.RecordNotFound);
            }
            if (_productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return opration.Failed(ApplicationMessages.DuplicatedRecord);
            }
            var slug = command.Slug.Slugify();
            var categorySlug = _productCategoryRepository.GetSlugById(command.CategoryId);
            var path = $"{categorySlug}/{slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            getDetails.Edit(command.Name, command.Code, command.ShortDescription,
                command.Description,picturePath, command.PictureAlt, command.PictureTitle, slug,
                command.KeyWords, command.MetaDescription, command.CategoryId);
            _productRepository.SaveChanges();

            return opration.Succeeded();
        }

        public EditProduct GetDetails(long Id)
        {
            var product =  _productRepository.GetDetails(Id);
            var editProduct = new EditProduct
            {
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Id = product.Id,
                PictureTitle = product.PictureTitle,
                PictureAlt = product.PictureAlt,
                //Picture = product.Picture,
                KeyWords = product.KeyWords,
                MetaDescription = product.MetaDescription,
                Slug = product.Slug,
                ShortDescription = product.ShortDescription,
                //UnitPrice = product.UnitPrice,

            };
            return editProduct;
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {

            var listProduct = _productRepository.Search(searchModel.Name, searchModel.Code, searchModel.CategoryId);
            var listProductViewModel = listProduct.Select(x => new ProductViewModel
            {
                Name = x.Name,
                Code = x.Code,
                Id = x.Id,
                Picture = x.Picture,
                //UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                Category = x.Category.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                //IsInStock = x.IsInStock,



            }).ToList();

            return listProductViewModel;
        }

      

        public List<ProductViewModel> GetProducts()
        {

            var listProduct = _productRepository.GetProducts();
            var listProductViewModel = listProduct.Select(x => new ProductViewModel
            {
                Name = x.Name,
                Id = x.Id,

            }).ToList();

            return listProductViewModel;
        }
    }
}
