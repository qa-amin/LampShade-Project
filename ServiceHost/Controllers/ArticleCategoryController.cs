using _1_LampshadeQuery.Contracts.Article;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ArticleCategoryController : Controller
    {
        
        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public ArticleCategoryController(IArticleCategoryQuery articleCategoryQuery, IArticleQuery articleQuery)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }
        [HttpGet]
        [Route("ArticleCategory/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var latestArticles = await _articleQuery.LatestArticles();
            ViewBag.LatestArticles = latestArticles;

            var articleCategory = await _articleCategoryQuery.GetArticleCategory(id);
            ViewBag.ArticleCategory = articleCategory;

            var articleCategories = await _articleCategoryQuery.GetArticleCategories();
            ViewBag.ArticleCategories = articleCategories;

            return View("Index");
        }
    }
}
