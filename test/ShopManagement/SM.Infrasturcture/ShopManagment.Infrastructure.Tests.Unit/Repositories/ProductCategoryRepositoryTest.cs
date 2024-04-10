using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

    }
}
