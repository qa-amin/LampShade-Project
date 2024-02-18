using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleCategoryRepository : RepositoryBase<long, ArticleCategory>, IArticleCategoryRepository
    {
        private readonly BlogManagementDbContext _context;

        public ArticleCategoryRepository(BlogManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ArticleCategoryViewModel>> GetArticleCategories()
        {
            return await _context.ArticleCategories.Select(x => new ArticleCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
        }

        public async Task<EditArticleCategory> GetDetails(long id)
        {
            return await _context.ArticleCategories.Select(x => new EditArticleCategory
            {
                Id = x.Id,
                Name = x.Name,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                ShowOrder = x.ShowOrder,
                Slug = x.Slug,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> GetSlugBy(long id)
        {
            var articleCategory =  await _context.ArticleCategories.Select(x => new { x.Id, x.Slug })
                .FirstOrDefaultAsync(x => x.Id == id);
            return articleCategory.Slug;
        }

        public async Task<List<ArticleCategoryViewModel>> Search(ArticleCategorySearchModel searchModel)
        {
            var query = _context.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Picture = x.Picture,
                    ShowOrder = x.ShowOrder,
                    CreationDate = x.CreationDate.ToFarsi(),
                    ArticlesCount = x.Articles.Count
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return await query.OrderByDescending(x => x.ShowOrder).ToListAsync();
        }
    }
}
