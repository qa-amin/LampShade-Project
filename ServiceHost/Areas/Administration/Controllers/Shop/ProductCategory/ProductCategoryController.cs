using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Shop.ProductCategory
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        private  List<ProductCategoryViewModel> _productCategoryVeiwModels = new List<ProductCategoryViewModel>();

        public ProductCategorySearchModel searchModel;

        public ProductCategoryController(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
           
        }


        [Area("Administration")]
        [Route("admin/shop/productcategory/index")]
        [HttpGet]
        public IActionResult Index(string? searchModel)
        {
            var SearchModel = new ProductCategorySearchModel
            {
                Name = searchModel
            };
            _productCategoryVeiwModels = _productCategoryApplication.Search(SearchModel);
            return View(_productCategoryVeiwModels);
        }


        [Area("Administration")]
        [Route("admin/shop/productcategory/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new CreateProductCategory());
        }

        [Area("Administration")]
        [Route("admin/shop/productcategory/Create")]
        [HttpPost]
        public JsonResult Create(CreateProductCategory commend)
        {
            var result = _productCategoryApplication.Create(commend);

            return new JsonResult(result);
        }


        [Area("Administration")]
        [Route("admin/shop/productcategory/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editProductCategory = _productCategoryApplication.GetDetails(Id);
            return PartialView("_Edit", editProductCategory);
        }

        [Area("Administration")]
        [Route("admin/shop/productcategory/Edit")]
        
        public JsonResult Edit(EditProductCategory commend)
        {
            var result = _productCategoryApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}
