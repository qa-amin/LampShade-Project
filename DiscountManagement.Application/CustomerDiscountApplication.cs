using _0_Framework.Application;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;
        

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository)
        {
            _customerDiscountRepository = customerDiscountRepository;
            
        }

        public async Task<OperationResult> Define(DefineCustomerDiscount command)
        {
            var operationResult = new OperationResult();
            if (await _customerDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate, startDate, 
                endDate, command.Reason);

            await _customerDiscountRepository.Create(customerDiscount);
            await _customerDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Edit(EditCustomerDiscount command)
        {
            var operationResult = new OperationResult();

            var customerDiscount = await _customerDiscountRepository.Get(command.Id);
            if (customerDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _customerDiscountRepository.Exists(x =>
                    x.ProductId == command.ProductId && x.DiscountRate == command.ProductId && x.Id != command.Id))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();

            customerDiscount.Edit(command.Id, command.DiscountRate, startDate, endDate,command.Reason);
            await _customerDiscountRepository.SaveChanges();
            return operationResult.Succeeded();

        }

        public async Task<List<CustomerDiscountViewModel>> search(CustomerDiscountSearchModel searchModel)
        {
            return await _customerDiscountRepository.search(searchModel.ProductId, searchModel.StartDate,
                searchModel.EndDate);
        }

        public async Task<EditCustomerDiscount> GetDetails(long id)
        {
            return await _customerDiscountRepository.GetDetails(id);
        }
    }
}
