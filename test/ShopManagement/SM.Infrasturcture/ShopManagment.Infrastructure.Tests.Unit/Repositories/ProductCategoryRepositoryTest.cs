using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ShopManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagment.Domain.Tests.Unit.ProductCategoryAgg;

namespace ShopManagment.Infrastructure.Tests.Unit.Repositories
{
    public class ProductCategoryRepositoryTest
    {
        private readonly ShopManagementDbContext _context;
        private readonly ProductCategoryRepository _productCategoryRepository;
        private readonly ProductCategoryBuilder _productCategoryBuilder;

        public ProductCategoryRepositoryTest()
        {
            DbContextOptionsBuilder<ShopManagementDbContext> dbOptions =
                new DbContextOptionsBuilder<ShopManagementDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString() // Use GUID so every test will use a different db
                    );
            _context = new ShopManagementDbContext(dbOptions.Options);
            _productCategoryRepository = new ProductCategoryRepository(_context);
            _productCategoryBuilder = new ProductCategoryBuilder();

        }


        [Fact]
        public async Task Create_ShouldAddProductCategoryToDb()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();

            
            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();
            var productCategoryDb = await _context.ProductCategories.FirstOrDefaultAsync();

            //Assert
            productCategory.Id.Should().Be(productCategoryDb.Id);
            

        }


        [Fact]
        public async Task Create_ShouldNotBeEqual_WhenIdIsDifferent()
        {
            //Arrange
            var productCategory1 = _productCategoryBuilder.Build();
            var productCategory2 = _productCategoryBuilder.Build();


            //Act
            await _productCategoryRepository.Create(productCategory1);
            await _productCategoryRepository.Create(productCategory2);
            await _productCategoryRepository.SaveChanges();
            var productCategoryDb1 = await _context.ProductCategories.FirstOrDefaultAsync();
            var productCategoryDb2 = await _context.ProductCategories.LastOrDefaultAsync();

            //Assert
            productCategory1.Id.Should().NotBe(productCategoryDb2.Id);


        }

        [Fact]
        public async Task Get_ShouldReturnListOfAllProductCategoryFromDb()
        {
            //Arrange
            var productCategory1 = _productCategoryBuilder.Build();
            var productCategory2 = _productCategoryBuilder.Build();
            var productCategory3 = _productCategoryBuilder.Build();

            //Act
            await _productCategoryRepository.Create(productCategory1);
            await _productCategoryRepository.Create(productCategory2);
            await _productCategoryRepository.Create(productCategory3);
            await _productCategoryRepository.SaveChanges();
            var productCategories = await _productCategoryRepository.Get();


            //Assert
            productCategories.Count.Should().BeGreaterThanOrEqualTo(3);


        }

        [Fact]
        public async Task Get_ShouldReturnProductCategory_WhenGivesId()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();

            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();
            var productCategoryWithIdOne = await _productCategoryRepository.Get(1);

            //Assert
            productCategoryWithIdOne.Id.Should().Be(productCategory.Id);
            
            
        }

        [Fact]
        public async Task GetSlugById_ShouldReturnSlugOfProductCategory_WhenGivesId()
        {
            //Arrange
            var productCategory =  _productCategoryBuilder.Build();

            
            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var slug = await _productCategoryRepository.GetSlugById(1);

            //Assert
            slug.Should().Be(productCategory.Slug);
        }

        [Fact]
        public async Task Search_ShouldReturnProductCategory_WhenGivesSearchModelLikeBook()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();
            const string searchModel = "Book";

            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var productCategoryWithSearch = await _productCategoryRepository.Search(searchModel);

            //Assert
            productCategoryWithSearch.Count.Should().Be(1);

        }

        [Fact]
        public async Task Search_ShouldReturnListOfProductCategory_WhenGivesSearchModelLikeBook()
        {
            //Arrange
            var productCategory1 = _productCategoryBuilder.Build();
            var productCategory2 = _productCategoryBuilder.Build();
            const string searchModel = "Book";


            //Act
            await _productCategoryRepository.Create(productCategory1);
            await _productCategoryRepository.Create(productCategory2);
            await _productCategoryRepository.SaveChanges();

            var productCategoryWithSearch = await _productCategoryRepository.Search(searchModel);

            //Assert
            productCategoryWithSearch.Count.Should().Be(2);

        }

        [Fact]
        public async Task Search_ShouldNotReturnProductCategoryOrListOfProductCategory_WhenGivesSearchModelLikePen()
        {
            //Arrange
            var productCategory1 = _productCategoryBuilder.Build();
            var productCategory2 = _productCategoryBuilder.Build();
            const string searchModel = "Pen";


            //Act
            await _productCategoryRepository.Create(productCategory1);
            await _productCategoryRepository.Create(productCategory2);
            await _productCategoryRepository.SaveChanges();

            var productCategoryWithSearch = await _productCategoryRepository.Search(searchModel);

            //Assert
            productCategoryWithSearch.Count.Should().Be(0);

        }

        [Fact]
        public async Task Exists_ShouldReturnTrue_WhenGivesNameThatExistsInDb()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();
            const string name = "Book";

            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var exists = await _productCategoryRepository.Exists(x => x.Name == name);

            //Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task Exists_ShouldReturnFalse_WhenGivesNameThatNotExistsInDb()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();
            const string name = "Book1";

            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var exists = await _productCategoryRepository.Exists(x => x.Name == name);

            //Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task Exists_ShouldReturnFalse_WhenGivesNameThatExistsWithDifferentIdInDb()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();
            const int id = 1;
            const string name = "Book";


            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var exists = await _productCategoryRepository.Exists(x => x.Name == name && x.Id != id);

            //Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetDetails_ShouldReturnEditProductCategory_WhenGivesId()
        {
            //Arrange
            var productCategory = _productCategoryBuilder.Build();


            //Act
            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.SaveChanges();

            var editProductCategory = await _productCategoryRepository.GetDetails(productCategory.Id);


            //Assert
            editProductCategory.Id.Should().Be(productCategory.Id);
            editProductCategory.Slug.Should().Be(productCategory.Slug);
        }

        [Fact]
        public async Task GetProductCategories_ShouldReturnListOfProductCategoryViewModel()
        {
            //Arrange
            var productCategory1 = _productCategoryBuilder.Build();
            var productCategory2 = _productCategoryBuilder.Build();


            //Act
            await _productCategoryRepository.Create(productCategory1);
            await _productCategoryRepository.Create(productCategory2);
            await _productCategoryRepository.SaveChanges();

            var listProductCategoryViewModel = await _productCategoryRepository.GetProductCategories();

            //Assert
            listProductCategoryViewModel.Should().HaveCountGreaterThanOrEqualTo(2);
        }
    }
}
