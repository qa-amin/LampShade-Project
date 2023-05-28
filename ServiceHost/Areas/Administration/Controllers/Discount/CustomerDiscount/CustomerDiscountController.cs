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
        [Route("admin/discount/customerdiscount/index")]
        [HttpGet]
        public IActionResult Index(long? productId, string? startDate, string? endDate)
        {
            var serchModel = new CustomerDiscountSearchModel
            {
                EndDate = endDate,
                StartDate = startDate,
                ProductId = productId
            };

            _customerDiscountViewModelList = _customerDiscountApplication.search(serchModel);

            var products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            ViewBag.products = products;

            return View(_customerDiscountViewModelList);
        }



        [Area("Administration")]
        [Route("admin/discount/customerdiscount/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new DefineCustomerDiscount();
            command.Products = _productApplication.GetProducts();

            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/discount/customerdiscount/Create")]
        [HttpPost]
        public JsonResult Create(DefineCustomerDiscount commend)
        {
            var result = _customerDiscountApplication.Define(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/discount/customerdiscount/Edit")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var editCustomerDiscount = _customerDiscountApplication.GetDetails(id);
            editCustomerDiscount.Products = _productApplication.GetProducts();

            return PartialView("_Edit", editCustomerDiscount);
        }

        [Area("Administration")]
        [Route("admin/discount/customerdiscount/Edit")]

        public JsonResult Edit(EditCustomerDiscount commend)
        {
            var result = _customerDiscountApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}
