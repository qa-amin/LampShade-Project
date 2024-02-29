using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;


namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthHelper _authHelper;
        public AccountApplication(IFileUploader fileUploader, IAccountRepository accountRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IAuthHelper authHelper)
        {
            _fileUploader = fileUploader;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _authHelper = authHelper;
        }

        public async Task<OperationResult> ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = await _accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);

            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);
            await _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<AccountViewModel> GetAccountBy(long id)
        {
            var account = await _accountRepository.Get(id);
            return new AccountViewModel()
            {
                Fullname = account.Fullname,
                Mobile = account.Mobile
            };
        }

        public async Task<OperationResult> Register(RegisterAccount command)
        {
            var operation = new OperationResult();

            if (await _accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var password = _passwordHasher.Hash(command.Password);
            var path = $"profilePhotos";
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);
            var account = new Account(command.Fullname, command.Username, password, command.Mobile, command.RoleId,
                picturePath);
            await _accountRepository.Create(account);
            await _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = await _accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (await _accountRepository.Exists(x =>
                (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var path = $"profilePhotos";
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);
            account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, picturePath);
            await _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<EditAccount> GetDetails(long id)
        {
            return await _accountRepository.GetDetails(id);
        }

        public async Task<OperationResult> Login(Login command)
        {
            var operation = new OperationResult();
            var account = await _accountRepository.GetBy(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var result = _passwordHasher.Check(account.Password, command.Password);
            if (!result.Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var role = await _roleRepository.Get(account.RoleId);
            var permissions = role.Permissions.Select(x => x.Code).ToList();

            var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.Fullname
                , account.Username, account.Mobile,permissions, account.ProfilePhoto);

            _authHelper.Signin(authViewModel);
            return operation.Succeeded();
        }

        public async Task Logout()
        {
             _authHelper.SignOut();
        }

        public Task<List<AccountViewModel>> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public Task<List<AccountViewModel>> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}