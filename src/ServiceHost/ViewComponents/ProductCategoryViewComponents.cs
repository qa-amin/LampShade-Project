using _1_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class ProductCategoryViewComponents : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductCategoryViewComponents(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productCategoryQuery = await _productCategoryQuery.GetProductCategories();
            return View(productCategoryQuery);
        }
    }
}
