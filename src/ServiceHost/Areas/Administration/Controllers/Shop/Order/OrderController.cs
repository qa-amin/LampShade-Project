using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Order;
using Controller = Microsoft.AspNetCore.Mvc.Controller;


namespace ServiceHost.Areas.Administration.Controllers.Shop.Order
{
    
    public class OrderController : Controller
    {
        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;

        public OrderController( IAccountApplication accountApplication, IOrderApplication orderApplication)
        {
            _accountApplication = accountApplication;
            _orderApplication = orderApplication;
        }
        [Area("Administration")]
        [Route("admin/orders")]
        [HttpGet]
        public async Task<IActionResult> Index(long? accountId, bool? isCanceled)
        {
            var searchModel = new OrderSearchModel
            {
                AccountId = accountId,
                IsCanceled = isCanceled
            };
            var accounts = await _accountApplication.GetAccounts();
            ViewBag.Accounts = accounts;

            var order = await _orderApplication.Search(searchModel);
            return View(order);
        }
        [Area("Administration")]
        [Route("admin/order/Cancel")]
        [HttpGet]
        public async Task<IActionResult> Cancel(long id)
        {
            await _orderApplication.Cancel(id);

            return RedirectToAction("Index","Order",new{AdminArea = "Administration" });
        }
        [Area("Administration")]
        [Route("admin/order/Confirm")]
        [HttpGet]
        public async Task<IActionResult> Confirm(long id)
        {
            await _orderApplication.PaymentSucceeded(id, 0);

            return RedirectToAction("Index");
        }
        [Area("Administration")]
        [Route("admin/order/Items")]
        [HttpGet]
        public async Task<IActionResult> Items(long id)
        {
            var items = await _orderApplication.GetItems(id);
            return PartialView("Items", items);
        }
    }
}
