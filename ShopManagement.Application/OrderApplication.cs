using System.Collections.Generic;
using _0_Framework.Application;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;

        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShopInventoryAcl _shopInventoryAcl;
        //private readonly ISmsService _smsService;
        //private readonly IShopAccountAcl _shopAccountAcl;

        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IAccountRepository accountRepository, IProductRepository productRepository, IShopInventoryAcl shopInventoryAcl)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
            _shopInventoryAcl = shopInventoryAcl;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            }

            _orderRepository.Create(order);
            _orderRepository.SaveChanges();
            return order.Id;
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public void Cancel(long id)
        {
            var order = _orderRepository.Get(id);
            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public string PaymentSucceeded(long orderId, long refId)
        {
            var order = _orderRepository.Get(orderId);
            order.PaymentSucceeded(refId);
            var symbol = _configuration["Symbol"];
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

            _orderRepository.SaveChanges();

            //var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);

            //_smsService.Send(mobile,
            //    $"{name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
            return issueTrackingNo;
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            var products = _productRepository.Get().Select(x => new { x.Id, x.Name }).ToList();
            var orderItem = _orderRepository.GetItems(orderId);
            var items = orderItem.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).ToList();

            foreach (var item in items)
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
            }
            foreach (var item in items)
            {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
            }

            return items;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var accounts = _accountRepository.Get().Select(x => new { x.Id, x.Fullname }).ToList();
            var search =  _orderRepository.Search(searchModel.AccountId, searchModel.IsCanceled);
            var query = search.Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                DiscountAmount = x.DiscountAmount,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.IsCanceled != null) query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0) query = query.Where(x => x.AccountId == searchModel.AccountId);

            var orders = query.OrderByDescending(x => x.Id).ToList();
            foreach (var order in orders)
            {
                order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.Fullname;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
            }

            return orders;
        }
    }
}