using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Area("Administration")]
        [Route("admin/roles")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleApplication.List();
            //var accountRole = new SelectList(_roleApplication.List(), "Id", "Name");
            //ViewBag.AccountRole = accountRole;

            return View(roles);
        }


        [Area("Administration")]
        [Route("admin/role/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateRole();
            
           
            return View("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/role/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRole commend)
        {
            var result = await _roleApplication.Create(commend);

            return RedirectToAction("Index");
        }



        [Area("Administration")]
        [Route("admin/role/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var Command = await _roleApplication.GetDetails(Id);
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
        [Route("admin/role/Edit")]
        public async Task<IActionResult> Edit(EditRole commend)
        {
            var result = await _roleApplication.Edit(commend);
            return RedirectToAction("Index");
        }

       
		
    }
}
