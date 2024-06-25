using System.Linq.Expressions;
using _0_Framework.Application;
using FluentAssertions;
using NSubstitute;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Tests.Unit.BuilderPatterns;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagment.Domain.Tests.Unit.ProductCategoryAgg;

namespace ShopManagement.Application.Tests.Unit
{
    public class ProductCategoryApplicationTest
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IFileUploader _filleUploader;
        private readonly ProductCategoryApplication _productCategoryApplication;
        private readonly CreateProductCategoryBuilder _createProductCategoryBuilder;
        private readonly EditProductCategoryBuilder _editProductCategoryBuilder;
        private readonly ProductCategoryBuilder _productCategoryBuilder;
        public ProductCategoryApplicationTest()
        {
            _productCategoryRepository = Substitute.For<IProductCategoryRepository>();
            _filleUploader = Substitute.For<IFileUploader>();
            _productCategoryApplication = new ProductCategoryApplication(_productCategoryRepository, _filleUploader);
           
            _createProductCategoryBuilder = new CreateProductCategoryBuilder();
            _editProductCategoryBuilder = new EditProductCategoryBuilder();
            _productCategoryBuilder = new ProductCategoryBuilder();
        }


        [Fact]
        public async Task Create_ShouldCreateProductCategoryAndCallCreateProductCategoryRepository()
        {
            //Arrange
            var createProductCategory = _createProductCategoryBuilder.Build();
            

            //Act
             await _productCategoryApplication.Create(createProductCategory);

            //Assert
            await _productCategoryRepository.ReceivedWithAnyArgs().Create(default);


        }

        [Fact]
        public async Task Create_ShouldCreateProductCategoryAndReturnOperationResult()
        {
            //Arrange
            var createProductCategory = _createProductCategoryBuilder.Build();
            var operation = new OperationResult();


            //Act
            operation =  await _productCategoryApplication.Create(createProductCategory);

            //Assert
            operation.IsSucceeded.Should().BeTrue();


        }

        [Fact]
        public async Task Create_ShouldReturnFalseOperation_WhenAddingProductCategoryIsDuplicated()
        {
            //Arrange
            var createProductCategory = _createProductCategoryBuilder.Build();
            var operation = new OperationResult();

            _productCategoryRepository
                .Exists(Arg.Any<Expression<Func<ProductCategory, bool>>>())
                .Returns(true);


            //Act

            operation = await _productCategoryApplication.Create(createProductCategory);
           

            

            //Assert
            operation.IsSucceeded.Should().BeFalse();


        }


        [Fact]
        public async Task Edit_ShouldReturnTrueOperationResult()
        {
            //Arrange
            var editProductCategory = _editProductCategoryBuilder.Build();
            var operation = new OperationResult();

            _productCategoryRepository.Get(Arg.Any<long>())
                .Returns(_productCategoryBuilder.Build());


            //Act

            operation = await _productCategoryApplication.Edit(editProductCategory);

            //Assert
            operation.IsSucceeded.Should().BeTrue();


        }

        [Fact]
        public async Task Edit_ShouldReturnFalseOperationResult_WhenProductCategoryNameIsDuplicate()
        {
            //Arrange
            var editProductCategory = _editProductCategoryBuilder.Build();
            var operation = new OperationResult();

            _productCategoryRepository.Get(Arg.Any<long>())
                .Returns(_productCategoryBuilder.Build());

            _productCategoryRepository
                .Exists(Arg.Any<Expression<Func<ProductCategory, bool>>>())
                .Returns(true);


            //Act

            operation = await _productCategoryApplication.Edit(editProductCategory);

            //Assert
            operation.IsSucceeded.Should().BeFalse();


        }

        [Fact]
        public async Task Edit_ShouldReturnFalseOperationResult_WhenProductCategoryIsNull()
        {
            //Arrange
            var editProductCategory = _editProductCategoryBuilder.Build();
            var operation = new OperationResult();

            _productCategoryRepository
                .Exists(Arg.Any<Expression<Func<ProductCategory, bool>>>())
                .Returns(false);


            //Act

            operation = await _productCategoryApplication.Edit(editProductCategory);

            //Assert
            operation.IsSucceeded.Should().BeFalse();


        }


        [Fact]
        public async Task GetProductCategories_ShouldReturnListOfProductCategoryViewModel()
        {
            //Arrange
            _productCategoryRepository.GetProductCategories()
                .Returns(new List<ProductCategoryViewModel>());
            //Act
            var listProductCategoryViewModel = await _productCategoryApplication.GetProductCategories();


            //Assert
            listProductCategoryViewModel.Should().BeOfType<List<ProductCategoryViewModel>>();
            await _productCategoryRepository.ReceivedWithAnyArgs().GetProductCategories();


        }

        [Fact]
        public async Task GetDetails_ShouldReturnEditProductCategory()
        {
            //Arrange
            _productCategoryRepository.GetDetails(Arg.Any<long>())
                .Returns(_editProductCategoryBuilder.Build());

            //Act
            var editProductCategory = await _productCategoryApplication.GetDetails(default);

            editProductCategory.Should().BeOfType<EditProductCategory>();
            await _productCategoryRepository.ReceivedWithAnyArgs().GetDetails(default);
        }

        [Fact]
        public async Task Search_ShouldReturnListOfProductCategoryViewModel()
        {
            //Arrange
            _productCategoryRepository.Search(Arg.Any<string>())
                .Returns(new List<ProductCategoryViewModel>());

            //Act
            var listProductCategoryViewModel = await _productCategoryApplication.Search(new ProductCategorySearchModel());

            listProductCategoryViewModel.Should().BeOfType<List<ProductCategoryViewModel>>();
            await _productCategoryRepository.ReceivedWithAnyArgs().Search(default);
        }
    }
}
