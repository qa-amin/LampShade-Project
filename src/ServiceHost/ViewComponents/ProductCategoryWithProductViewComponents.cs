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

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _productCategoryQuery.GetProductCategoriesWithProducts();
			return View(categories);
		}
	}
}
