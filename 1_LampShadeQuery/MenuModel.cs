
using System.Collections.Generic;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using _1_LampShadeQuery.Contracts.ProductCategory;

namespace _01_LampshadeQuery
{
    public class MenuModel
    {
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
        public List<ProductCategoryQueryModel> ProductCategories { get; set; }
    }
}
