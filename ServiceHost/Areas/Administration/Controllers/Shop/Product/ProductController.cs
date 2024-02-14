using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Shop.Product
{
    [Authorize(Policy = "Administration")]
    public class ProductController : Controller
    {
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;

        private List<ProductViewModel> _productViewModels = new List<ProductViewModel>();


        [TempData]
        public string Message { get; set; }



        public ProductController(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        [Area("Administration")]
        [Route("admin/products")]
        [HttpGet]
        public async Task<IActionResult> Index(string? name, string? code, long? categoryId)
        {
            var SearchModel = new ProductSearchModel
            {
                Name = name,
                Code = code,
                CategoryId = categoryId
            };
            var productCategories = new SelectList(await _productCategoryApplication.GetProductCategories(), "Id", "Name");
            ViewBag.ProductCategories = productCategories;
            
            _productViewModels = await _productApplication.Search(SearchModel);
            return View(_productViewModels);
        }


        [Area("Administration")]
        [Route("admin/product/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateProduct();
            command.Categoreis = await _productCategoryApplication.GetProductCategories();
           
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/product/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateProduct commend)
        {
            var result = await _productApplication.Create(commend);
            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/product/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var editProduct =await _productApplication.GetDetails(id);
            editProduct.Categoreis = await _productCategoryApplication.GetProductCategories();
            
            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/product/Edit")]

        public async Task<JsonResult> Edit(EditProduct commend)
        {
            var result = await _productApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}
