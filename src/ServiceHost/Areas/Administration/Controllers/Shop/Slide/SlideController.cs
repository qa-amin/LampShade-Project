using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Controllers.Shop.Slide
{
    [Authorize(Policy = "Administration")]
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
        [Route("admin/slides")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _slideViewModels = await _slideApplication.GetList();
            return View(_slideViewModels);
        }


        [Area("Administration")]
        [Route("admin/slide/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateSlide();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/slide/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateSlide commend)
        {
            var result = await _slideApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/slide/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var editProduct = await _slideApplication.GetDetails(Id);
            

            return PartialView("_Edit", editProduct);
        }

        [Area("Administration")]
        [Route("admin/slide/Edit")]

        public async Task<JsonResult> Edit(EditSlide commend)
        {
            var result = await _slideApplication.Edit(commend);
            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/slide/Remove")]

        public async Task<IActionResult> Remove(long id)
        {
            var result = await _slideApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/slide/Restore")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _slideApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }
    }
}
