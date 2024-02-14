using _0_Framework.Domain;
using DiscountManagement.Application.Contracts.CustomerDiscount;

namespace DiscountManagement.Domain.CustomerDiscountAgg
{
    public interface ICustomerDiscountRepository : IRepository<long, CustomerDiscount>
    {
        Task<List<CustomerDiscountViewModel>> search(long? productId, string? startDate, string? endDate);
        Task<EditCustomerDiscount> GetDetails(long id);
    }
}
