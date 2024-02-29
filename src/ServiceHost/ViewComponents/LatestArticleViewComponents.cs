using _1_LampshadeQuery.Contracts.Article;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articleQueryModel = await _articleQuery.LatestArticles();
            return View(articleQueryModel);
        }
    }
}
