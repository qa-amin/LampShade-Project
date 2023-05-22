using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using ShopManagement.Domain.ProductAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository _colleagueDiscountRepository;
        private readonly IProductRepository _productRepository;

        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository, IProductRepository productRepository)
        {
            _colleagueDiscountRepository = colleagueDiscountRepository;
            _productRepository = productRepository;
        }

        public OperationResult Define(DefineColleagueDiscount command)
        {
            var operationResult = new OperationResult();
            if (_colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.ProductId))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

           
            var colleagueDiscount = new ColleagueDiscount(command.ProductId, command.DiscountRate);

            _colleagueDiscountRepository.Create(colleagueDiscount);
            _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Edit(EditColleagueDiscount command)
        {

            var operationResult = new OperationResult();

            var colleagueDiscount = _colleagueDiscountRepository.GetDetails(command.Id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_colleagueDiscountRepository.Exists(x =>
                    x.ProductId == command.ProductId && x.DiscountRate == command.ProductId && x.Id != command.Id))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }



            colleagueDiscount.Edit(command.Id, command.DiscountRate);
            _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Remove(long id)
        {
            var operationResult = new OperationResult();

            var colleagueDiscount = _colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }
           



            colleagueDiscount.Remove();
            _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Restore(long id)
        {
            var operationResult = new OperationResult();

            var colleagueDiscount = _colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }




            colleagueDiscount.Restore();
            _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public List<ColleagueDiscountViewModel> search(ColleagueDiscountSearchModel searchModel)
        {
            var products = _productRepository.Get().Select(x => new { x.Id, x.Name }).ToList();

            var query = _colleagueDiscountRepository.Get().Select(x => new ColleagueDiscountViewModel()
            {
                DiscountRate = x.DiscountRate,
                Id = x.Id,
                ProductId = x.ProductId,
                CreationDate = x.CreationDate.ToFarsi(),
                IsRemoved = x.IsRemoved


            }).ToList();

            if (searchModel.ProductId > 0 && searchModel.ProductId != null)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId).ToList();
            }

            

            var discounts = query.OrderByDescending(x => x.Id).ToList();

            discounts.ForEach(discount => discount.Product = products.FirstOrDefault(p => p.Id == discount.ProductId)?.Name);

            return discounts;
        }

        public EditColleagueDiscount GetDetails(long id)
        {
            var colleagueDiscount = _colleagueDiscountRepository.GetDetails(id);

            var editCustomerDiscount = new EditColleagueDiscount()
            {
                DiscountRate = colleagueDiscount.DiscountRate,
                
                Id = colleagueDiscount.Id,
                ProductId = colleagueDiscount.ProductId,
                
            };

            return editCustomerDiscount;
        }
    }
}
