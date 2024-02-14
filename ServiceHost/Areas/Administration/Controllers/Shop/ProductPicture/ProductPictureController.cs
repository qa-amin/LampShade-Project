using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ServiceHost.Areas.Administration.Controllers.Shop.ProductPicture
{
    [Authorize(Policy = "Administration")]
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
        [Route("admin/productpictures")]
        [HttpGet]
        public async Task<IActionResult> Index(long? productId)
        {
            var productPictureSerchModel = new ProductPictureSearchModel
            {
                ProductId = productId
            };

            var products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            ViewBag.Products = products;


            _productsPictureViewModel = await _productPictureApplication.Search(productPictureSerchModel);

            return View(_productsPictureViewModel);
        }




        [Area("Administration")]
        [Route("admin/productpicture/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateProductPicture();

            command.Products = await _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/productpicture/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateProductPicture commend)
        {
            var result = await _productPictureApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/productpicture/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var editProduct = await _productPictureApplication.GetDetails(Id);
            editProduct.Products = await _productApplication.GetProducts();

            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/productpicture/Edit")]

        public async Task<JsonResult> Edit(EditProductPicture commend)
        {
            var result = await _productPictureApplication.Edit(commend);
            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/productpicture/Remove")]

        public async Task<IActionResult> Remove(long id)
        {
            var result = await _productPictureApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/productpicture/Restore")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _productPictureApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }
    }
}
