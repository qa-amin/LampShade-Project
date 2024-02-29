using System.Collections.Generic;


namespace _1_LampshadeQuery.Contracts.Article
{
    public interface IArticleQuery
    {
        Task<List<ArticleQueryModel>> LatestArticles();
        Task<ArticleQueryModel> GetArticleDetails(string slug);
    }
}
