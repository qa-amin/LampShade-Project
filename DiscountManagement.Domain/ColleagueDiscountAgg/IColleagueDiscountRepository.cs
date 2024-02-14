using _0_Framework.Domain;
using DiscountManagement.Application.Contracts.ColleagueDiscount;

namespace DiscountManagement.Domain.ColleagueDiscountAgg
{
    public interface IColleagueDiscountRepository : IRepository<long , ColleagueDiscount>
    {
       Task<List<ColleagueDiscountViewModel>> Search(long id);
       Task<EditColleagueDiscount> GetDetails(long id);
    }
}
