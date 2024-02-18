using _0_Framework.Domain;
using BlogManagement.Application.Contracts.Article;

namespace BlogManagement.Domain.ArticleAgg
{
    public interface IArticleRepository : IRepository<long, Article>
    {
        Task<EditArticle> GetDetails(long id);
        Task<Article> GetWithCategory(long id);
        Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel);
    }
}
