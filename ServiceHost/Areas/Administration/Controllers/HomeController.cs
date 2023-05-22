using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
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
