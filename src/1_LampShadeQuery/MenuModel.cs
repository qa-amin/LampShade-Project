
using System.Collections.Generic;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using _1_LampShadeQuery.Contracts.ProductCategory;

namespace _1_LampShadeQuery
{
    public class MenuModel
    {
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
        public List<ProductCategoryQueryModel> ProductCategories { get; set; }
    }
}
