using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestArrivalsViewComponents : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public LatestArrivalsViewComponents(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public async Task<IViewComponentResult> Invoke()
        {
            var productQueryModel = await  _productQuery.GetLatestArrivals();
            return View(productQueryModel);
        }
    }
}
