using _1_LampShadeQuery.Contracts.Product;
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

        private ProductCategoryQueryModel productCategoryQueryModel;

        

        [Route("productcategory/index/{id}")]
        public IActionResult Index(string id)
        {
            productCategoryQueryModel = _productCategoryQuery.GetProductCategoryWithProductsBy(id);

            return View(productCategoryQueryModel);
        }
    }
}
