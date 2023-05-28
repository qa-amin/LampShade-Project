using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountApplication _accountApplication;

        
        public string LoginMessage { get; set; }
        

        
        public string RegisterMessage { get; set; }
        public AccountController(IAccountApplication accountApplication)
        {
            ViewBag.LoginMessage = LoginMessage;
            ViewBag.RegisterMessage = RegisterMessage;
            _accountApplication = accountApplication;
        }

        [Route("account")]
		public IActionResult Index()
        {
            return View();
        }
        [Route("account/login")]
        public IActionResult Login(string username, string password)
        {
            var command = new Login
            {
                Password = password,
                Username = username,
            };
            var result = _accountApplication.Login(command);
            if (result.IsSucceeded)
                return RedirectToAction("Index");

            ViewBag.LoginMessage = result.Message;
            return RedirectToAction("Index");
        }


        [Route("account/logout")]
        public IActionResult LogOut()
        {
            
            _accountApplication.Logout();

            return RedirectToAction("Index");
        }

        [Route("account/Register")]
        public IActionResult Register(RegisterAccount command)
        {
           var result = _accountApplication.Register(command);
           if (result.IsSucceeded)
               return RedirectToAction("Login");

           ViewBag.RegisterMessage = result.Message;

           return RedirectToAction("Index");
        }

    }
}
