using _0_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Shop.Product
{
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
        [Route("admin/shop/product/index")]
        [HttpGet]
        public IActionResult Index(string? name, string? code, long? categoryId)
        {
            var SearchModel = new ProductSearchModel
            {
                Name = name,
                Code = code,
                CategoryId = categoryId
            };
            var productCategories = new SelectList(_productCategoryApplication.GetProductCategories(), "Id", "Name");
            ViewBag.ProductCategories = productCategories;
            
            _productViewModels = _productApplication.Search(SearchModel);
            return View(_productViewModels);
        }


        [Area("Administration")]
        [Route("admin/shop/product/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateProduct();
            command.Categoreis = _productCategoryApplication.GetProductCategories();
           
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/shop/product/Create")]
        [HttpPost]
        public JsonResult Create(CreateProduct commend)
        {
            var result = _productApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/shop/product/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editProduct = _productApplication.GetDetails(Id);
            editProduct.Categoreis = _productCategoryApplication.GetProductCategories();
            
            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/shop/product/Edit")]

        public JsonResult Edit(EditProduct commend)
        {
            var result = _productApplication.Edit(commend);
            return new JsonResult(result);
        }

        //[Area("Administration")]
        //[Route("admin/shop/product/InStock")]

        //public IActionResult InStock(long id)
        //{
        //   var result =  _productApplication.IsStock(id);
        //   if (result.IsSucceeded)
        //       return Redirect("./index");
           
        //   Message = result.Message;
        //   return Redirect("./index");
        //}

        //[Area("Administration")]
        //[Route("admin/shop/product/NotInStock")]
        //public IActionResult NotInStock(long id)
        //{
        //    var result = _productApplication.NotInStock(id);
        //    if (result.IsSucceeded)
        //        return Redirect("./index");

        //    Message = result.Message;
        //    return Redirect("./index");
        //}
    }
}
