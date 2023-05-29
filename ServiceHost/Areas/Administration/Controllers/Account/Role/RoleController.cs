using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ServiceHost.Areas.Administration.Controllers.Account.Role
{
    [Authorize(Policy = "Administration")]
    public class RoleController : Controller
    {
        
        private readonly IRoleApplication _roleApplication;

        public List<SelectListItem> Permissions = new List<SelectListItem>();

        private readonly IEnumerable<IPermissionExposer> _exposers;




        public RoleController(IRoleApplication roleApplication, IEnumerable<IPermissionExposer> exposers)
        {
            _roleApplication = roleApplication;
            _exposers = exposers;
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
            
           
            return View("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/account/role/Create")]
        [HttpPost]
        public IActionResult Create(CreateRole commend)
        {
            var result = _roleApplication.Create(commend);

            return RedirectToAction("Index");
        }



        [Area("Administration")]
        [Route("admin/account/role/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var Command = _roleApplication.GetDetails(Id);
            foreach (var exposer in _exposers)
            {
                var exposedPermissions = exposer.Expose();
                foreach (var (key, value) in exposedPermissions)
                {
                    var group = new SelectListGroup { Name = key };
                    foreach (var permission in value)
                    {
                        var item = new SelectListItem(permission.Name, permission.Code.ToString())
                        {
                            Group = group
                        };

                        if (Command.MappedPermissions.Any(x => x.Code == permission.Code))
                            item.Selected = true;

                        Permissions.Add(item);
                    }
                }
            }
            ViewBag.Permissions = Permissions;
            return View("_Edit", Command);
        }

        [Area("Administration")]
        [Route("admin/account/role/Edit")]

        public IActionResult Edit(EditRole commend)
        {
            var result = _roleApplication.Edit(commend);
            return RedirectToAction("Index");
        }

       
		
    }
}
