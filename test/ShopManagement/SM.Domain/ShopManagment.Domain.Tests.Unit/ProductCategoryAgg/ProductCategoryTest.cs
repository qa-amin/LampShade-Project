using FluentAssertions;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagment.Domain.Tests.Unit.ProductCategoryAgg
{
    public class ProductCategoryTest
    {
        [Fact]
        public void Constructor_ShouldConstructProductCategoryProperty()
        {
            var productCategoryBuilder = new ProductCategoryBuilder();
            var productCategory = productCategoryBuilder.Build();


            productCategory.Name.Should().Be(productCategoryBuilder.Name);
            productCategory.Description.Should().Be(productCategoryBuilder.Description);
            productCategory.Picture.Should().Be(productCategoryBuilder.Picture);
            productCategory.PictureAlt.Should().Be(productCategoryBuilder.PictureAlt);
            productCategory.PictureTitle.Should().Be(productCategoryBuilder.PictureTitle);
            productCategory.KeyWords.Should().Be(productCategoryBuilder.KeyWords);
            productCategory.MetaDescription.Should().Be(productCategory.MetaDescription);
            productCategory.Slug.Should().Be(productCategoryBuilder.Slug);

        }

        [Fact]
        public void Edit_ShouldChangeNameProperty()
        {
            var productCategoryBuilder = new ProductCategoryBuilder();
            var productCategory = productCategoryBuilder.Build();

            var newName = "Book1";

            productCategory.Edit(newName, productCategoryBuilder.Description,
                productCategoryBuilder.Picture, productCategoryBuilder.PictureAlt,
                productCategoryBuilder.PictureTitle, productCategoryBuilder.KeyWords,
                productCategoryBuilder.MetaDescription,
                productCategoryBuilder.Slug);

            productCategory.Name.Should().NotBe(productCategoryBuilder.Name);
            productCategory.Name.Should().Be(newName);
        }
    }
}
