using System.Collections.Generic;
using _1_LampshadeQuery.Contracts.ArticleCategory;

namespace _1_LampshadeQuery.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        Task<ArticleCategoryQueryModel> GetArticleCategory(string slug);
        Task<List<ArticleCategoryQueryModel>> GetArticleCategories();
    }
}
