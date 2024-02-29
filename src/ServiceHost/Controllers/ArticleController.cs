using _1_LampshadeQuery.Contracts.Article;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ArticleController : Controller
    {
        
        
        private readonly IArticleQuery _articleQuery;

         private readonly IArticleCategoryQuery _articleCategoryQuery;

        public ArticleController(IArticleQuery articleQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }
        

        [HttpGet]
        [Route("Article/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var article = await _articleQuery.GetArticleDetails(id);
            ViewBag.Article = article;

            var latestArticles = await _articleQuery.LatestArticles();
            ViewBag.LatestArticles = latestArticles;

            var articleCategories = await _articleCategoryQuery.GetArticleCategories();
            ViewBag.articleCategories = articleCategories;

            return View();
        }
    }
}
