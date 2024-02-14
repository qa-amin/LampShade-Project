using _0_Framework.Application;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountManagement.Application.Contracts.ColleagueDiscount
{
    public interface IColleagueDiscountApplication
    {
        Task<OperationResult> Define(DefineColleagueDiscount command);
        Task<OperationResult> Edit(EditColleagueDiscount command);
        Task<OperationResult> Remove(long id);
        Task<OperationResult> Restore(long id);
        Task<List<ColleagueDiscountViewModel>> search(ColleagueDiscountSearchModel searchModel);
        Task<EditColleagueDiscount> GetDetails(long id);
    }
}
