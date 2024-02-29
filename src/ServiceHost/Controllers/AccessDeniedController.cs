using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class AccessDeniedController : Controller
    {
        [Route("AccessDenied")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
