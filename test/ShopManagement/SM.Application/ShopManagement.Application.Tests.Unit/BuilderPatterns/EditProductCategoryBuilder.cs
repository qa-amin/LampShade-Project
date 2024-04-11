using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Application.Tests.Unit.BuilderPatterns
{
    public class EditProductCategoryBuilder : CreateProductCategoryBuilder
    {
        public long Id { get; set; } = 1;

        public EditProductCategory Build()
        {
            var editProductCategory = new EditProductCategory();
            editProductCategory.Name = Name;
            editProductCategory.Description = Description;
            editProductCategory.Picture = Picture;
            editProductCategory.Slug = Slug;
            editProductCategory.KeyWords = KeyWords;
            editProductCategory.MetaDescription = MetaDescription;
            editProductCategory.PictureTitle = PictureTitle;
            editProductCategory.Picture = Picture;
            editProductCategory.Id = Id;
            return editProductCategory;
        }
    }
}
