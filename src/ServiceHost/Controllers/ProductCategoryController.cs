using _1_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProductCategoryController(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }


        [Route("productcategory/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var productCategoryQueryModel = await _productCategoryQuery.GetProductCategoryWithProductsBy(id);

            return View(productCategoryQueryModel);
        }
    }
}
