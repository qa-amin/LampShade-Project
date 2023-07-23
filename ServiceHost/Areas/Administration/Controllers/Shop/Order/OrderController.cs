using System.Web.Mvc;
using AccountManagement.Application.Contracts.Account;
using DocumentFormat.OpenXml.Office2010.Excel;
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
        [Microsoft.AspNetCore.Mvc.Route("admin/shop/order/index")]
        [System.Web.Mvc.HttpGet]
        public IActionResult Index(long? accountId, bool? isCanceled)
        {
            var searchModel = new OrderSearchModel
            {
                AccountId = accountId,
                IsCanceled = isCanceled
            };
            var accounts = _accountApplication.GetAccounts();
            ViewBag.Accounts = accounts;

            var order = _orderApplication.Search(searchModel);
            return View(order);
        }
        [Area("Administration")]
        [Microsoft.AspNetCore.Mvc.Route("admin/shop/order/Cancel")]
        [System.Web.Mvc.HttpGet]
        public IActionResult Cancel(long id)
        {
            _orderApplication.Cancel(id);

            return RedirectToAction("Index","Order",new{AdminArea = "Administration" });
        }
        [Area("Administration")]
        [Microsoft.AspNetCore.Mvc.Route("admin/shop/order/Confirm")]
        [System.Web.Mvc.HttpGet]
        public IActionResult Confirm(long id)
        {
            _orderApplication.PaymentSucceeded(id, 0);

            return RedirectToAction("Index");
        }
        [Area("Administration")]
        [Microsoft.AspNetCore.Mvc.Route("admin/shop/order/Items")]
        [System.Web.Mvc.HttpGet]
        public IActionResult Items(long id)
        {
            var items = _orderApplication.GetItems(id);
            return PartialView("Items", items);
        }
    }
}
