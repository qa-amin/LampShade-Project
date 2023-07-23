using _0_Framework.Application;
using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;
using System.Globalization;
using _01_LampshadeQuery.Contracts;
using _0_Framework.Application.ZarinPal;
using Microsoft.AspNetCore.Authorization;
using ShopManagement.Application;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ServiceHost.Controllers
{
    [Authorize(Policy = "evrey")]
    public class CheckoutController : Controller
    {
        
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly IZarinPalFactory _zarinPalFactory;

        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutController(ICartCalculatorService cartCalculatorService, ICartService cartService,
            IProductQuery productQuery,
            IAuthHelper authHelper, IZarinPalFactory zarinPalFactory, IOrderApplication orderApplication)
        {
            Cart = new Cart();
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            
            
            _authHelper = authHelper;
            _zarinPalFactory = zarinPalFactory;
            _orderApplication = orderApplication;
        }

        [Route("/checkout/index")]
        public IActionResult Index()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);

            return View(Cart);
        }


        public IActionResult Pay(int paymentMethod)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);
            var result = _productQuery.CheckInventoryStatus(cart.Items);
            if (result.Any(x => !x.IsInStock))
                return RedirectToAction("Index", "Checkout");


            var orderId = _orderApplication.PlaceOrder(cart);
            if (paymentMethod == 1)
            {
                return RedirectToAction("CallBack", "Checkout", new { oId = orderId });
            }
            else
            {
                var resultPayment = new PaymentResult();
                resultPayment.Succeeded(
                    "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد.", null);
                
                Response.Cookies.Delete("cart-items");
                return RedirectToAction("PaymentResult", resultPayment);
            }





            //if (paymentMethod == 1)
            //{
            //    var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
            //        cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
            //        "خرید از درگاه لوازم خانگی و دکوری", orderId);

            //    return Redirect(
            //        $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");


            //}

            
        }
        [Route("Checkout/CallBack")]
        public IActionResult CallBack(long oId)
        {
            var operation = new OperationResult();
            var code = 1212;
            var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, code);
            var result = new PaymentResult();
            result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
            Response.Cookies.Delete("cart-items");
            return RedirectToAction("PaymentResult",result);
        }

        public IActionResult PaymentResult(PaymentResult result)
        {
            return View(result);
        }

    }
}

