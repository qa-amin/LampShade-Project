using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Domain.ProductAgg;

namespace ServiceHost.Controllers
{
    public class SearchController : Controller
    {
        private readonly IProductQuery _productQuery;
        public List<ProductQueryModel>? Products { get; set; }

        public SearchController(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        [HttpPost]
        [Route("search/index")]
        public IActionResult Index(string id)
        {
            Products = _productQuery.Search(id);
            ViewBag.Products = Products;
            ViewBag.Search = id;
            return View();
        }

      

    }
}
