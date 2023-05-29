using _0_Framework.Application;
using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;
using System.Globalization;
using _01_LampshadeQuery.Contracts;

namespace ServiceHost.Controllers
{
    public class CheckoutController : Controller
    {
        
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        
        //private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutController(ICartCalculatorService cartCalculatorService, ICartService cartService,
            IProductQuery productQuery,
            IAuthHelper authHelper)
        {
            Cart = new Cart();
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            
            
            _authHelper = authHelper;
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

       
    }
}

