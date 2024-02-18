using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleRepository : RepositoryBase<long, Article>, IArticleRepository
    {
        private readonly BlogManagementDbContext _context;

        public ArticleRepository(BlogManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EditArticle> GetDetails(long id)
        {
            return await _context.Articles.Select(x => new EditArticle
            {
                Id = x.Id,
                CanonicalAddress = x.CanonicalAddress,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription,
                Slug = x.Slug,
                Title = x.Title
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Article> GetWithCategory(long id)
        {
            return await _context.Articles.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ArticleViewModel>> Search(ArticleSearchModel searchModel)
        {
            var query = _context.Articles.Select(x => new ArticleViewModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Category = x.Category.Name,
                Picture = x.Picture,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 50)) + " ...",
                Title = x.Title
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
                query = query.Where(x => x.Title.Contains(searchModel.Title));

            if (searchModel.CategoryId > 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);

            return await query.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
