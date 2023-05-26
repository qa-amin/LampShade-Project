using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ServiceHost.Areas.Administration.Controllers.Shop.Product
{
    public class AccountController : Controller
    {
        private readonly IAccountApplication _accountApplication;
        

        private List<AccountViewModel> _accountViewModels = new List<AccountViewModel>();


        public AccountController(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
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
            //var accountRole = new SelectList(_accountApplication., "Id", "Name");
            //ViewBag.AccountRole = accountRole;
            
            _accountViewModels = _accountApplication.Search(SearchModel);
            return View(_accountViewModels);
        }


        [Area("Administration")]
        [Route("admin/account/account/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new RegisterAccount();
            command.Roles = new List<RoleViewModel>
            {
	            new RoleViewModel
	            {
		            Id = 1,
		            CreationDate = DateTime.Now.ToFarsi(),
		            Name = "مدیرسیستم"
	            }
            };
           
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

            editAccount.Roles = new List<RoleViewModel>
			{
				new RoleViewModel
				{
					Id = 1,
					CreationDate = DateTime.Now.ToFarsi(),
					Name = "مدیرسیستم"
				}
			};
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
