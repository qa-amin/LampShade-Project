using _1_LampshadeQuery.Contracts.Article;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ArticleCategoryController : Controller
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> LatestArticles;

        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public ArticleCategoryController(IArticleCategoryQuery articleCategoryQuery, IArticleQuery articleQuery)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }
        [HttpGet]
        [Route("ArticleCategory/index/{id}")]
        public IActionResult Index(string id)
        {
            LatestArticles = _articleQuery.LatestArticles();
            ViewBag.LatestArticles = LatestArticles;

            ArticleCategory = _articleCategoryQuery.GetArticleCategory(id);
            ViewBag.ArticleCategory = ArticleCategory;

            ArticleCategories = _articleCategoryQuery.GetArticleCategories();
            ViewBag.ArticleCategories = ArticleCategories;

            return View("Index");
        }
    }
}
