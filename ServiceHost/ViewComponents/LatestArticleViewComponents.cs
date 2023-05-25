using _1_LampshadeQuery.Contracts.Article;
using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestArticleViewComponents : ViewComponent
    {
        private readonly IArticleQuery _articleQuery;

        public LatestArticleViewComponents(IArticleQuery articleQuery)
        {
            _articleQuery = articleQuery;
        }

        public IViewComponentResult Invoke()
        {
            var articleQueryModel = _articleQuery.LatestArticles();
            return View(articleQueryModel);
        }
    }
}
