using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Controllers.Account.Role
{
    public class RoleController : Controller
    {
        
        private readonly IRoleApplication _roleApplication;
        

        


        public RoleController(IRoleApplication roleApplication)
        {
	        
	        _roleApplication = roleApplication;
        }

        [TempData]
        public string Message { get; set; }



        

        [Area("Administration")]
        [Route("admin/account/role/index")]
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleApplication.List();
            //var accountRole = new SelectList(_roleApplication.List(), "Id", "Name");
            //ViewBag.AccountRole = accountRole;

            return View(roles);
        }


        [Area("Administration")]
        [Route("admin/account/role/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateRole();
            
           
            return PartialView("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/account/role/Create")]
        [HttpPost]
        public JsonResult Create(CreateRole commend)
        {
            var result = _roleApplication.Create(commend);

            return new JsonResult(result);
        }



        [Area("Administration")]
        [Route("admin/account/role/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editAccount = _roleApplication.GetDetails(Id);
            return PartialView("_Edit", editAccount);
        }

        [Area("Administration")]
        [Route("admin/account/role/Edit")]

        public JsonResult Edit(EditRole commend)
        {
            var result = _roleApplication.Edit(commend);
            return new JsonResult(result);
        }

       
		
    }
}
