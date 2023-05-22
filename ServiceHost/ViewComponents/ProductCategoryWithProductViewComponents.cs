using _1_LampShadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
	public class ProductCategoryWithProductViewComponents : ViewComponent
	{
		private readonly IProductCategoryQuery _productCategoryQuery;

		public ProductCategoryWithProductViewComponents(IProductCategoryQuery productCategoryQuery)
		{
			_productCategoryQuery = productCategoryQuery;
		}

		public IViewComponentResult Invoke()
		{
			var categories = _productCategoryQuery.GetProductCategoriesWithProducts();
			return View(categories);
		}
	}
}
