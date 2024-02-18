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


        public async Task<IViewComponentResult> Invoke()
        {
            var menuModel = new MenuModel
            {
                ProductCategories = await _productCategoryQuery.GetProductCategories(),
                ArticleCategories = await _articleCategoryQuery.GetArticleCategories(),
            };
            return View(menuModel);
        }
    }
}
