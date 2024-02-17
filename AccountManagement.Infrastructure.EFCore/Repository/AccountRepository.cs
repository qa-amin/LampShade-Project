using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<long, Account>, IAccountRepository
    {
        private readonly AccountManagementDbContext _accountManagementDbContext;

        public AccountRepository(AccountManagementDbContext accountManagementDbContext) : base(accountManagementDbContext)
        {
            _accountManagementDbContext = accountManagementDbContext;
        }

        public async Task<Account> GetBy(string username)
        {
            return await _accountManagementDbContext.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<EditAccount> GetDetails(long id)
        {
            return await _accountManagementDbContext.Accounts.Select(x => new EditAccount
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Mobile = x.Mobile,
                RoleId = x.RoleId,
                Username = x.Username
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AccountViewModel>> GetAccounts()
        {
            return _accountManagementDbContext.Accounts.Select(x => new AccountViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname
            }).ToList();
        }

        public async Task<List<AccountViewModel>> Search(AccountSearchModel searchModel)
        {
            var query =  _accountManagementDbContext.Accounts.Include(x => x.Role).Select(x => new AccountViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Mobile = x.Mobile,
                ProfilePhoto = x.ProfilePhoto,
                Role = x.Role.Name,
                RoleId = x.RoleId,
                Username = x.Username,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
                query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

            if (!string.IsNullOrWhiteSpace(searchModel.Username))
                query = query.Where(x => x.Username.Contains(searchModel.Username));

            if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));

            if (searchModel.RoleId > 0)
                query = query.Where(x => x.RoleId == searchModel.RoleId);

            return await query.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}