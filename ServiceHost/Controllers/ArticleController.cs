using _1_LampshadeQuery.Contracts.Article;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using BlogManagement.Application.Contracts.ArticleCategory;
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
        [Route("Article/index/{id}")]
        public IActionResult Index(string id)
        {
            var Article = _articleQuery.GetArticleDetails(id);
            ViewBag.Article = Article;

            var LatestArticles = _articleQuery.LatestArticles();
            ViewBag.LatestArticles = LatestArticles;

            var articleCategories = _articleCategoryQuery.GetArticleCategories();
            ViewBag.articleCategories = articleCategories;

            return View();
        }
    }
}
