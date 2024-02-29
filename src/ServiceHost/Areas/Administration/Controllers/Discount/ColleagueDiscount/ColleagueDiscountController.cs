using DiscountManagement.Application.Contracts.ColleagueDiscount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Controllers.Discount.ColleagueDiscount
{
    [Authorize(Policy = "Administration")]
    public class ColleagueDiscountController : Controller
    {
        private readonly IColleagueDiscountApplication _colleagueDiscountApplication;
        private readonly IProductApplication _productApplication;

        private List<ColleagueDiscountViewModel> _customerDiscountViewModelList = new List<ColleagueDiscountViewModel>();


        public ColleagueDiscountController(IColleagueDiscountApplication colleagueDiscountApplication, IProductApplication productApplication)
        {
            _colleagueDiscountApplication = colleagueDiscountApplication;
            _productApplication = productApplication;
        }

        [TempData]
        public string Message { get; set; }


        [Area("Administration")]
        [Route("admin/colleaguediscount/index")]
        [HttpGet]
        public async Task<IActionResult> Index(long? productId)
        {
            var serchModel = new ColleagueDiscountSearchModel()
            {
                
                ProductId = productId
            };

            _customerDiscountViewModelList = await _colleagueDiscountApplication.search(serchModel);

            var products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(_customerDiscountViewModelList);
        }



        [Area("Administration")]
        [Route("admin/colleaguediscount/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new DefineColleagueDiscount();
            command.Products = await _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/colleaguediscount/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(DefineColleagueDiscount commend)
        {
            var result = await _colleagueDiscountApplication.Define(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/colleaguediscount/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var editColleagueDiscount = await _colleagueDiscountApplication.GetDetails(id);
            editColleagueDiscount.Products = await _productApplication.GetProducts();

            return PartialView("_Edit", editColleagueDiscount);
        }

        [Area("Administration")]
        [Route("admin/colleaguediscount/Edit")]

        public async Task<JsonResult> Edit(EditColleagueDiscount commend)
        {
            var result = await _colleagueDiscountApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/colleaguediscount/Remove")]

        public async Task<IActionResult> Remove(long id)
        {
            var result = await _colleagueDiscountApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/colleaguediscount/Restore")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _colleagueDiscountApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }


    }

    
}
