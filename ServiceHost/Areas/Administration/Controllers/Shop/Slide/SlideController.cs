using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Controllers.Shop.Slide
{
    public class SlideController : Controller
    {
        private readonly ISlideApplication _slideApplication;
        private List<SlideViewModel> _slideViewModels = new List<SlideViewModel>();
        public SlideController(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        [TempData]
        public string Message { get; set; }

        [Area("Administration")]
        [Route("admin/shop/slide/index")]
        [HttpGet]
        public IActionResult Index()
        {
            _slideViewModels = _slideApplication.GetList();
            return View(_slideViewModels);
        }


        [Area("Administration")]
        [Route("admin/shop/slide/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateSlide();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/shop/slide/Create")]
        [HttpPost]
        public JsonResult Create(CreateSlide commend)
        {
            var result = _slideApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/shop/slide/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editProduct = _slideApplication.GetDetails(Id);
            

            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/shop/slide/Edit")]

        public JsonResult Edit(EditSlide commend)
        {
            var result = _slideApplication.Edit(commend);
            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/shop/slide/Remove")]

        public IActionResult Remove(long id)
        {
            var result = _slideApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/shop/slide/Restore")]
        public IActionResult Restore(long id)
        {
            var result = _slideApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }
    }
}
