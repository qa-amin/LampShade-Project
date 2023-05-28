using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountApplication _accountApplication;

        [TempData]
        public string LoginMessage { get; set; }

        [TempData]
        public string RegisterMessage { get; set; }
        public AccountController(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("account/login")]
        public IActionResult Login(Login command)
        {
            var result = _accountApplication.Login(command);
            if (result.IsSucceeded)
                return RedirectToPage("/Index");

            ViewBag.LoginMessage = result.Message;
            return RedirectToPage("/Account");
        }
    }
}
