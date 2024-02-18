using Microsoft.EntityFrameworkCore;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _1_LampShadeQuery.Contracts;
using DiscountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Order;

namespace _1_LampShadeQuery.Query
{
    public class CartCalculatorService : ICartCalculatorService
    {
        private readonly IAuthHelper _authHelper;
        private readonly DiscountManagementDbContext _discountManagementDbContext;

        public CartCalculatorService(DiscountManagementDbContext discountManagementDbContext, IAuthHelper authHelper)
        {
            _discountManagementDbContext = discountManagementDbContext;
            _authHelper = authHelper;
        }

        public async Task<Cart> ComputeCart(List<CartItem> cartItems)
        {
            var cart = new Cart();
            var colleagueDiscounts = await _discountManagementDbContext.ColleagueDiscounts
                .Where(x => !x.IsRemoved)
                .Select(x => new {x.DiscountRate, x.ProductId})
                .ToListAsync();

            var customerDiscounts = await _discountManagementDbContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new {x.DiscountRate, x.ProductId})
                .ToListAsync();
            var currentAccountRole = _authHelper.CurrentAccountRole();

            foreach (var cartItem in cartItems)
            {
                if (currentAccountRole == Roles.ColleagueUser)
                {
                    var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                    if (colleagueDiscount != null)
                        cartItem.DiscountRate = colleagueDiscount.DiscountRate;
                }
                else
                {
                    var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                    if (customerDiscount != null)
                        cartItem.DiscountRate = customerDiscount.DiscountRate;
                }

                cartItem.DiscountAmount = ((cartItem.TotalItemPrice * cartItem.DiscountRate) / 100);
                cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
                cart.Add(cartItem);
            }

            return cart;
        }
    }
}