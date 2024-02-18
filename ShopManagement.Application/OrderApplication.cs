using _0_Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IShopInventoryAcl _shopInventoryAcl;
        //private readonly ISmsService _smsService;
        //private readonly IShopAccountAcl _shopAccountAcl;

        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _shopInventoryAcl = shopInventoryAcl;
        }

        public async Task<long> PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            }

            await _orderRepository.Create(order);
            await _orderRepository.SaveChanges();
            return order.Id;
        }

        public async Task<double> GetAmountBy(long id)
        {
            return await _orderRepository.GetAmountBy(id);
        }

        public async Task Cancel(long id)
        {
            var order = await _orderRepository.Get(id);
            order.Cancel();
            await _orderRepository.SaveChanges();
        }

        public async Task<string> PaymentSucceeded(long orderId, long refId)
        {
            var order = await _orderRepository.Get(orderId);
            order.PaymentSucceeded(refId);
            var symbol = _configuration["Symbol"];
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            if (! await _shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

            await _orderRepository.SaveChanges();

            //var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);

            //_smsService.Send(mobile,
            //    $"{name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
            return issueTrackingNo;
        }

        public async Task<List<OrderItemViewModel>> GetItems(long orderId)
        {
            return await _orderRepository.GetItems(orderId);
        }

        public async Task<List<OrderViewModel>> Search(OrderSearchModel searchModel)
        {
            return await _orderRepository.Search(searchModel.AccountId, searchModel.IsCanceled);
        }
    }
}