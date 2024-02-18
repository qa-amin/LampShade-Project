﻿using _0_Framework.Application;
using _1_LampshadeQuery.Contracts.Article;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;


namespace _1_LampshadeQuery.Query
{
    public class ArticleCategoryQuery : IArticleCategoryQuery
    {
        private readonly BlogManagementDbContext _blogManagementDbContext;

        public ArticleCategoryQuery(BlogManagementDbContext blogManagementDbContext)
        {
            _blogManagementDbContext = blogManagementDbContext;
        }

        public async Task<List<ArticleCategoryQueryModel>> GetArticleCategories()
        {
            return await _blogManagementDbContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    ArticlesCount = x.Articles.Count
                }).ToListAsync();
        }

        public async Task<ArticleCategoryQueryModel> GetArticleCategory(string slug)
        {
            var articleCategory = await _blogManagementDbContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Description = x.Description,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    CanonicalAddress = x.CanonicalAddress,
                    ArticlesCount = x.Articles.Count,
                    Articles = MapArticles(x.Articles)
                }).FirstOrDefaultAsync(x => x.Slug == slug);

            if (!string.IsNullOrWhiteSpace(articleCategory.Keywords))
                articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();

            return articleCategory;
        }

        private static List<ArticleQueryModel> MapArticles(List<Article> articles)
        {
            return articles.Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                Title = x.Title,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
            }).ToList();
        }
    }
}
