using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Application.Tests.Unit.BuilderPatterns
{
    public class CreateProductCategoryBuilder
    {
        public string Name { get; private set; } = "Book";
        public string Description { get; private set; } = "This is a good picture";
        public IFormFile Picture { get; private set; } = new FormFile(Stream.Null, Int64.MaxValue, Int64.MaxValue, "Test","TestFileName");
        public string PictureAlt { get; private set; } = "Book Alt";
        public string PictureTitle { get; private set; } = "Book Title";
        public string KeyWords { get; private set; } = "Book KeyWords";
        public string MetaDescription { get; private set; } = "Book MetaDescription";
        public string Slug { get; private set; } = "Book Slug";
        public CreateProductCategory Build()
        {
           var createProductCategory = new CreateProductCategory();
           createProductCategory.Name = Name;
           createProductCategory.Description = Description;
           createProductCategory.Picture = Picture;
           createProductCategory.Slug = Slug;
           createProductCategory.KeyWords = KeyWords;
           createProductCategory.MetaDescription = MetaDescription;
           createProductCategory.PictureTitle = PictureTitle;
           createProductCategory.Picture = Picture;
           return createProductCategory;
        }
    }
}
