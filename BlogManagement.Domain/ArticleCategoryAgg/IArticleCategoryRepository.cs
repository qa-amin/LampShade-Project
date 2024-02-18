using _0_Framework.Domain;
using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Domain.ArticleCategoryAgg
{
    public interface IArticleCategoryRepository : IRepository<long, ArticleCategory>
    {
        Task<string> GetSlugBy(long id);
        Task<EditArticleCategory> GetDetails(long id);
        Task<List<ArticleCategoryViewModel>> GetArticleCategories();
        Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel);
    }
}
