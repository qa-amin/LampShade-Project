using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagment.Domain.Tests.Unit.ProductCategoryAgg
{
    public class ProductCategoryBuilder
    {
        public string Name { get; private set; } = "Book";
        public string Description { get; private set; } = "This is a good picture";
        public string Picture { get; private set; } = "نگهدارنده/2023-07-21-14-03-06-Product05.jpg";
        public string PictureAlt { get; private set; } = "Book Alt";
        public string PictureTitle { get; private set; } = "Book Title";
        public string KeyWords { get; private set; } = "Book KeyWords";
        public string MetaDescription { get; private set; } = "Book MetaDescription";
        public string Slug { get; private set; } = "Book Slug";
        public ProductCategory Build()
        {
            return new ProductCategory(Name, Description, 
                Picture, PictureAlt, PictureTitle, KeyWords, MetaDescription,
                Slug);
        }

        

    }
}
