using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;
        private readonly IProductRepository _productRepository;

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository, IProductRepository productRepository)
        {
            _customerDiscountRepository = customerDiscountRepository;
            _productRepository = productRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            var operationResult = new OperationResult();
            if (_customerDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.ProductId))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate, startDate, 
                endDate, command.Reason);

            _customerDiscountRepository.Create(customerDiscount);
            _customerDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Edit(EditCustomerDiscount command)
        {
            var operationResult = new OperationResult();

            var customerDiscount = _customerDiscountRepository.GetDetails(command.Id);
            if (customerDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_customerDiscountRepository.Exists(x =>
                    x.ProductId == command.ProductId && x.DiscountRate == command.ProductId && x.Id != command.Id))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();

            customerDiscount.Edit(command.Id, command.DiscountRate, startDate, endDate,command.Reason);
            _customerDiscountRepository.SaveChanges();
            return operationResult.Succeeded();

        }

        public List<CustomerDiscountViewModel> search(CustomerDiscountSearchModel searchModel)
        {
            var products = _productRepository.Get().Select(x => new { x.Id, x.Name }).ToList();

            var query = _customerDiscountRepository.Get().Select(x => new CustomerDiscountViewModel
            {
                DiscountRate = x.DiscountRate,
                EndDateGr = x.EndDate,
                StartDateGr = x.StartDate,
                EndDate = x.EndDate.ToFarsi(),
                StartDate = x.StartDate.ToFarsi(),
                Id = x.Id,
                ProductId = x.ProductId,
                Reason = x.Reason,
                CreationDate = x.CreationDate.ToFarsi(),
                

            } ).ToList();

            if (searchModel.ProductId > 0 && searchModel.ProductId != null)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            {
                
                query = query.Where(x => x.StartDateGr >= searchModel.StartDate.ToGeorgianDateTime()).ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            {
                
                query = query.Where(x => x.EndDateGr <= searchModel.EndDate.ToGeorgianDateTime()).ToList();
            }

            var discounts = query.OrderByDescending(x => x.Id).ToList();

            discounts.ForEach(discount => discount.Product = products.FirstOrDefault(p => p.Id == discount.ProductId)?.Name);

            return discounts;

        }

        public EditCustomerDiscount GetDetails(long id)
        {
            var customerDiscount = _customerDiscountRepository.GetDetails(id);

            var editCustomerDiscount = new EditCustomerDiscount
            {
                DiscountRate = customerDiscount.DiscountRate,
                EndDate = customerDiscount.EndDate.ToFarsi(),
                StartDate = customerDiscount.StartDate.ToFarsi(),
                Id = customerDiscount.Id,
                ProductId = customerDiscount.ProductId,
                Reason = customerDiscount.Reason
            };

            return editCustomerDiscount;
        }
    }
}
