using System.Collections.Generic;
using _1_LampshadeQuery.Contracts.ArticleCategory;

namespace _1_LampshadeQuery.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        ArticleCategoryQueryModel GetArticleCategory(string slug);
        List<ArticleCategoryQueryModel> GetArticleCategories();
    }
}
