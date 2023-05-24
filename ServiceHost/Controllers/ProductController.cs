using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductController : Controller
    {
        public ProductQueryModel Product;
        private readonly IProductQuery _productQuery;

        public ProductController(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }
        [HttpGet]
        [Route("product/index/{id}")]
        public IActionResult Index(string id)
        {
            Product = _productQuery.GetProductDetails(id);
            ViewBag.Product = Product;
            return View();
        }
    }
}
