using ShopManagement.Application.Contracts.Order;

namespace _1_LampShadeQuery.Contracts
{
    public interface ICartCalculatorService
    {
        Task<Cart> ComputeCart(List<CartItem> cartItems);
    }
}