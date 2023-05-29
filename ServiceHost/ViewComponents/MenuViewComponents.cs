using _01_LampshadeQuery;
using _1_LampshadeQuery.Contracts.ArticleCategory;
using _1_LampShadeQuery.Contracts.Product;
using _1_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponents : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public MenuViewComponents(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }


        public IViewComponentResult Invoke()
        {
            var menuModel = new MenuModel
            {
                ProductCategories = _productCategoryQuery.GetProductCategories(),
                ArticleCategories = _articleCategoryQuery.GetArticleCategories(),
            };
            return View(menuModel);
        }
    }
}
