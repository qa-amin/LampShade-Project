using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagment.Domain.Tests.Unit.ProductCategoryAgg;

namespace ShopManagment.Infrastructure.Tests.Unit.Repositories
{
    public class ProductCategoryRepositoryTest
    {
        private readonly ShopManagementDbContext _context;

        public ProductCategoryRepositoryTest()
        {
            DbContextOptionsBuilder<ShopManagementDbContext> dbOptions =
                new DbContextOptionsBuilder<ShopManagementDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString() // Use GUID so every test will use a different db
                    );
            _context = new ShopManagementDbContext(dbOptions.Options);

        }


        [Fact]
        public async Task Create_ShouldAddProductCategoryToDb()
        {
            var productCategoryBuilder = new ProductCategoryBuilder();

            var productCategory = productCategoryBuilder.Build();

            var productCategoryRepository = new ProductCategoryRepository(_context);

            await productCategoryRepository.Create(productCategory);
            await productCategoryRepository.SaveChanges();

            var productCategoryDb = await _context.ProductCategories.FirstOrDefaultAsync();

            productCategory.Id.Should().Be(productCategoryDb.Id);
            

        }

    }
}
