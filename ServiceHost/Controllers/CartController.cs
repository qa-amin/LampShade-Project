using _1_LampShadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Controllers
{
    public class CartController : Controller
    {
        
        public const string CookieName = "cart-items";
        private readonly IProductQuery _productQuery;

        private readonly IHttpContextAccessor _contextAccessor;
        

        public CartController(IProductQuery productQuery, IHttpContextAccessor httpContextAccessor, IHttpContextAccessor contextAccessor)
        {
            _productQuery = productQuery;
            _contextAccessor = contextAccessor;
        }
        [Route("/Cart")]
        public async Task<IActionResult> Index()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            if (value == null)
            {
                ViewBag.cartItems = new List<CartItem>();
                return View(new List<CartItem>());
            }
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            if (cartItems != null)
            {
                foreach (var item in cartItems)
                    item.CalculateTotalItemPrice();
            }
            ViewBag.cartItems = cartItems;
            var CartItems = await _productQuery.CheckInventoryStatus(cartItems);
            return View(CartItems);
        }


        public IActionResult RemoveFromCart(long id)
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            cartItems.Remove(itemToRemove);
           
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
           
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);

            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> GoToCheckOut()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
            {
                item.TotalItemPrice = item.UnitPrice * item.Count;
            }

            var CartItems = await _productQuery.CheckInventoryStatus(cartItems);

            //if (CartItems.Any(x => !x.IsInStock))
            //    return RedirectToPage("/Cart");
            //return RedirectToPage("/Checkout");
            if (CartItems.Any(x => !x.IsInStock))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index","Checkout");
            }
            
        }
    }
}
