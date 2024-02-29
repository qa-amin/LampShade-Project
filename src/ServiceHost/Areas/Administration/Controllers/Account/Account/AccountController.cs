using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Controllers.Account.Account
{
    [Authorize(Policy = "Administration")]
    public class AccountController : Controller
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;
        

        private List<AccountViewModel> _accountViewModels = new List<AccountViewModel>();


        public AccountController(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
	        _accountApplication = accountApplication;
	        _roleApplication = roleApplication;
        }

        [Area("Administration")]
        [Route("admin/accounts")]
        [HttpGet]
        public async Task<IActionResult> Index(string? fullname, string? username, string? mobile,  long? roleId)
        {
            var searchModel = new AccountSearchModel()
            {
                Fullname = fullname,
                Mobile =mobile ,
                Username =username,
                RoleId = roleId
            };
            var accountRole = new SelectList(  await _roleApplication.List(), "Id", "Name");
            ViewBag.AccountRole = accountRole;

            _accountViewModels = await _accountApplication.Search(searchModel);
            return View(_accountViewModels);
        }


        [Area("Administration")]
        [Route("admin/account/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new RegisterAccount();
            command.Roles = await _roleApplication.List();
            
           
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/account/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(RegisterAccount commend)
        {
            var result = await _accountApplication.Register(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/account/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var editAccount = await _accountApplication.GetDetails(Id);

            editAccount.Roles = await _roleApplication.List();
			return PartialView("_Edit", editAccount);
        }

        [Area("Administration")]
        [Route("admin/account/Edit")]

        public async Task<JsonResult> Edit(EditAccount commend)
        {
            var result = await _accountApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/account/ChangePassword")]
        [HttpGet]

        public async Task<IActionResult> ChangePassword(long id)
        {
            var changePassword = new ChangePassword
            {
                Id = id
            };
            return PartialView("_ChangePassword", changePassword);
        }
        [Area("Administration")]
        [Route("admin/account/ChangePassword")]
        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePassword command)
        {
	        var result = await _accountApplication.ChangePassword(command);
			return new JsonResult(result);
		}

		
    }
}
