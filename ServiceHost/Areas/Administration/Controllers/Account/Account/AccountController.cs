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

        [TempData]
        public string Message { get; set; }



        

        [Area("Administration")]
        [Route("admin/account/account/index")]
        [HttpGet]
        public IActionResult Index(string? fullname, string? username, string? mobile,  long? roleId)
        {
            var SearchModel = new AccountSearchModel()
            {
                Fullname = fullname,
                Mobile =mobile ,
                Username =username,
                RoleId = roleId
            };
            var accountRole = new SelectList(_roleApplication.List(), "Id", "Name");
            ViewBag.AccountRole = accountRole;

            _accountViewModels = _accountApplication.Search(SearchModel);
            return View(_accountViewModels);
        }


        [Area("Administration")]
        [Route("admin/account/account/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new RegisterAccount();
            command.Roles = _roleApplication.List();
            
           
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/account/account/Create")]
        [HttpPost]
        public JsonResult Create(RegisterAccount commend)
        {
            var result = _accountApplication.Register(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/account/account/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editAccount = _accountApplication.GetDetails(Id);

            editAccount.Roles = _roleApplication.List();
			return PartialView("_Edit", editAccount);
        }

        [Area("Administration")]
        [Route("admin/account/account/Edit")]

        public JsonResult Edit(EditAccount commend)
        {
            var result = _accountApplication.Edit(commend);
            return new JsonResult(result);
        }

        [Area("Administration")]
        [Route("admin/account/account/ChangePassword")]
        [HttpGet]

        public IActionResult ChangePassword(long id)
        {
            var changePassword = new ChangePassword
            {
                Id = id
            };
            return PartialView("_ChangePassword", changePassword);
        }
        [Area("Administration")]
        [Route("admin/account/account/ChangePassword")]
        [HttpPost]
        public JsonResult ChangePassword(ChangePassword command)
        {
	        var result = _accountApplication.ChangePassword(command);
			return new JsonResult(result);
		}

		
    }
}
