using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
namespace ServiceHost.Controllers
{
    public class SearchController : Controller
    {
        private readonly IProductQuery _productQuery;
        public SearchController(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Index(string id)
        {
            var products = await _productQuery.Search(id);
            ViewBag.Products = products;
            ViewBag.Search = id;
            return View();
        }

      

    }
}
