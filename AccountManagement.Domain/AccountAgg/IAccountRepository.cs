using _0_Framework.Domain;
using AccountManagement.Application.Contracts.Account;

namespace AccountManagement.Domain.AccountAgg
{
    public interface IAccountRepository : IRepository<long, Account>
    {
        Task<Account> GetBy(string username);
        Task<EditAccount> GetDetails(long id);
        Task<List<AccountViewModel>> GetAccounts();
        Task<List<AccountViewModel>> Search(AccountSearchModel searchModel);
    }
}
