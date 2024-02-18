using _0_Framework.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application
{
    public class ArticleCategoryApplication : IArticleCategoryApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IArticleCategoryRepository _articleCategoryRepository;

        public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<OperationResult> Create(CreateArticleCategory command)
        {
            var operation = new OperationResult();
            if (await _articleCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var pictureName = _fileUploader.Upload(command.Picture, slug);
            var articleCategory = new ArticleCategory(command.Name, pictureName, command.PictureAlt, command.PictureTitle
                , command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);

            await _articleCategoryRepository.Create(articleCategory);
            await _articleCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Edit(EditArticleCategory command)
        {
            var operation = new OperationResult();
            var articleCategory = await _articleCategoryRepository.Get(command.Id);

            if (articleCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if ( await _articleCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var pictureName = _fileUploader.Upload(command.Picture, slug);
            articleCategory.Edit(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);

            await _articleCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<List<ArticleCategoryViewModel>> GetArticleCategories()
        {
            return await _articleCategoryRepository.GetArticleCategories();
        }

        public async Task<EditArticleCategory> GetDetails(long id)
        {
            return await _articleCategoryRepository.GetDetails(id);
        }

        public async Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel)
        {
            return await _articleCategoryRepository.Search(searchModel);
        }
    }

    
}
