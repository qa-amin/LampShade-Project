using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace DiscountManagement.Application.Contracts.CustomerDiscount
{
    public interface ICustomerDiscountApplication
    {
        Task<OperationResult> Define(DefineCustomerDiscount command);
        Task<OperationResult> Edit(EditCustomerDiscount command);
        Task<List<CustomerDiscountViewModel>> search(CustomerDiscountSearchModel searchModel);
        Task<EditCustomerDiscount> GetDetails(long id);
    }
}
