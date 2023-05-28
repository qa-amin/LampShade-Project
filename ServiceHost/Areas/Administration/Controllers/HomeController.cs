using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Authorize(Policy = "AdminArea")]
    public class HomeController : Controller
    {
        [Area("Administration")]
        [Route("admin/home/index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
