using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Application.Contracts.CustomerDiscount;
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
        [Route("admin/discount/colleaguediscount/index")]
        [HttpGet]
        public IActionResult Index(long? productId)
        {
            var serchModel = new ColleagueDiscountSearchModel()
            {
                
                ProductId = productId
            };

            _customerDiscountViewModelList = _colleagueDiscountApplication.search(serchModel);

            var products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(_customerDiscountViewModelList);
        }



        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new DefineColleagueDiscount();
            command.Products = _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Create")]
        [HttpPost]
        public JsonResult Create(DefineColleagueDiscount commend)
        {
            var result = _colleagueDiscountApplication.Define(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Edit")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var editColleagueDiscount = _colleagueDiscountApplication.GetDetails(id);
            editColleagueDiscount.Products = _productApplication.GetProducts();

            return PartialView("_Edit", editColleagueDiscount);
        }

        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Edit")]

        public JsonResult Edit(EditColleagueDiscount commend)
        {
            var result = _colleagueDiscountApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Remove")]

        public IActionResult Remove(long id)
        {
            var result = _colleagueDiscountApplication.Remove(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }

        [Area("Administration")]
        [Route("admin/discount/colleaguediscount/Restore")]
        public IActionResult Restore(long id)
        {
            var result = _colleagueDiscountApplication.Restore(id);
            if (result.IsSucceeded)
                return Redirect("./index");

            Message = result.Message;
            return Redirect("./index");
        }


    }

    
}
