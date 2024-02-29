using DiscountManagement.Application.Contracts.CustomerDiscount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Controllers.Discount.CustomerDiscount
{
    [Authorize(Policy = "Administration")]
    public class CustomerDiscountController : Controller
    {
        private readonly ICustomerDiscountApplication _customerDiscountApplication;
        private readonly IProductApplication _productApplication;

        private List<CustomerDiscountViewModel> _customerDiscountViewModelList = new List<CustomerDiscountViewModel>();

        

        public CustomerDiscountController(ICustomerDiscountApplication customerDiscountApplication, IProductApplication productApplication)
        {
            _customerDiscountApplication = customerDiscountApplication;
            _productApplication = productApplication;
        }

        [TempData]
        public string Message { get; set; }


        [Area("Administration")]
        [Route("admin/customerdiscounts")]
        [HttpGet]
        public async Task<IActionResult> Index(long? productId, string? startDate, string? endDate)
        {
            var serchModel = new CustomerDiscountSearchModel
            {
                EndDate = endDate,
                StartDate = startDate,
                ProductId = productId
            };

            _customerDiscountViewModelList = await _customerDiscountApplication.search(serchModel);

            var products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(_customerDiscountViewModelList);
        }



        [Area("Administration")]
        [Route("admin/customerdiscount/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new DefineCustomerDiscount();
            command.Products = await _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/customerdiscount/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(DefineCustomerDiscount commend)
        {
            var result = await _customerDiscountApplication.Define(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/customerdiscount/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var editCustomerDiscount = await _customerDiscountApplication.GetDetails(id);
            editCustomerDiscount.Products = await _productApplication.GetProducts();

            return PartialView("_Edit", editCustomerDiscount);
        }

        [Area("Administration")]
        [Route("admin/customerdiscount/Edit")]

        public async Task<JsonResult> Edit(EditCustomerDiscount commend)
        {
            var result = await _customerDiscountApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}
