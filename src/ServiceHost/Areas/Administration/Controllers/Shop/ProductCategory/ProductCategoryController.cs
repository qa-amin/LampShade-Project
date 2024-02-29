using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Shop.ProductCategory
{
    
    [Authorize(Policy = "Administration")]

    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        private  List<ProductCategoryViewModel> _productCategoryVeiwModels = new List<ProductCategoryViewModel>();


        public ProductCategoryController(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
           
        }


        [Area("Administration")]
        [Route("admin/productcategory")]
        [HttpGet]
        public async Task<IActionResult> Index(string? searchModel)
        {
            var SearchModel = new ProductCategorySearchModel
            {
                Name = searchModel
            };
            _productCategoryVeiwModels = await _productCategoryApplication.Search(SearchModel);
            return View(_productCategoryVeiwModels);
        }


        [Area("Administration")]
        [Route("admin/productcategory/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new CreateProductCategory());
        }

        [Area("Administration")]
        [Route("admin/productcategory/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateProductCategory commend)
        {
            var result = await _productCategoryApplication.Create(commend);

            return new JsonResult(result);
        }


        [Area("Administration")]
        [Route("admin/productcategory/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var editProductCategory = await _productCategoryApplication.GetDetails(id);
            return PartialView("_Edit", editProductCategory);
        }

        [Area("Administration")]
        [Route("admin/productcategory/Edit")]
        
        public async Task<JsonResult> Edit(EditProductCategory commend)
        {
            var result = await _productCategoryApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}
