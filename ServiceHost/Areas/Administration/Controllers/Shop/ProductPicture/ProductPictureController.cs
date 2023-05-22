using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ServiceHost.Areas.Administration.Controllers.Shop.ProductPicture
{
    public class ProductPictureController : Controller
    {
        private readonly IProductPictureApplication _productPictureApplication;
        private readonly IProductApplication _productApplication;
        private List<ProductPictureViewModel> _productsPictureViewModel = new List<ProductPictureViewModel>();


        [TempData]
        public string Message { get; set; }
        public ProductPictureController(IProductPictureApplication productPictureApplication, IProductApplication productApplication)
        {
            _productPictureApplication = productPictureApplication;
           
            _productApplication = productApplication;
        }

        [Area("Administration")]
        [Route("admin/shop/productpicture/index")]
        [HttpGet]
        public IActionResult Index(long? productId)
        {
            var productPictureSerchModel = new ProductPictureSearchModel
            {
                ProductId = productId
            };

            var products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            ViewBag.Products = products;


            _productsPictureViewModel = _productPictureApplication.Search(productPictureSerchModel);

            return View(_productsPictureViewModel);
        }




        [Area("Administration")]
        [Route("admin/shop/productpicture/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateProductPicture();

            command.Products = _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/shop/productpicture/Create")]
        [HttpPost]
        public JsonResult Create(CreateProductPicture commend)
        {
            var result = _productPictureApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/shop/productpicture/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editProduct = _productPictureApplication.GetDetails(Id);
            editProduct.Products = _productApplication.GetProducts();

            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/shop/productpicture/Edit")]

        public JsonResult Edit(EditProductPicture commend)
        {
            var result = _productPictureApplication.Edit(commend);
            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/shop/productpicture/Remove")]

        public IActionResult Remove(long id)
        {
            var result = _productPictureApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/shop/productpicture/Restore")]
        public IActionResult Restore(long id)
        {
            var result = _productPictureApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }
    }
}
