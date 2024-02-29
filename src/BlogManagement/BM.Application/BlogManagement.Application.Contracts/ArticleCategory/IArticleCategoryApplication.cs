using _0_Framework.Application;

namespace BlogManagement.Application.Contracts.ArticleCategory
{
    public interface IArticleCategoryApplication
    {
        Task<OperationResult> Create(CreateArticleCategory command);
        Task<OperationResult> Edit(EditArticleCategory command);
        Task<EditArticleCategory> GetDetails(long id);
        Task<List<ArticleCategoryViewModel>> GetArticleCategories();
        Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel);
    }
}
