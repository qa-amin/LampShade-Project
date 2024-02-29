using _0_Framework.Application;
using Microsoft.EntityFrameworkCore;
using _1_LampshadeQuery.Contracts.Article;
using BlogManagement.Infrastructure.EFCore;

namespace _1_LampshadeQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogManagementDbContext _blogManagementDbContext;

        public ArticleQuery(BlogManagementDbContext blogManagementDbContext)
        {
            _blogManagementDbContext = blogManagementDbContext;
        }


        public async Task<ArticleQueryModel> GetArticleDetails(string slug)
        {
            var article = await _blogManagementDbContext.Articles
               .Include(x => x.Category)
               .Where(x => x.PublishDate <= DateTime.Now)
               .Select(x => new ArticleQueryModel
               {
                   Id = x.Id,
                   Title = x.Title,
                   CategoryName = x.Category.Name,
                   CategorySlug = x.Category.Slug,
                   Slug = x.Slug,
                   CanonicalAddress = x.CanonicalAddress,
                   Description = x.Description,
                   Keywords = x.Keywords,
                   MetaDescription = x.MetaDescription,
                   Picture = x.Picture,
                   PictureAlt = x.PictureAlt,
                   PictureTitle = x.PictureTitle,
                   PublishDate = x.PublishDate.ToFarsi(),
                   ShortDescription = x.ShortDescription,
               }).FirstOrDefaultAsync(x => x.Slug == slug);

            if (!string.IsNullOrWhiteSpace(article.Keywords))
                article.KeywordList = article.Keywords.Split(",").ToList();

            return article;
        }

        public async Task<List<ArticleQueryModel>> LatestArticles()
        {
            return await _blogManagementDbContext.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate <= DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Title = x.Title,
                    Slug = x.Slug,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    PublishDate = x.PublishDate.ToFarsi(),
                    ShortDescription = x.ShortDescription,
                }).ToListAsync();
        }
    }
}
