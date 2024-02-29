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
           
            _accountApplication = accountApplication;
        }

        [Route("account")]
		public IActionResult Index()
        {
            return View();
        }
        [Route("account/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var command = new Login
            {
                Password = password,
                Username = username,
            };
            var result = await _accountApplication.Login(command);
            if (result.IsSucceeded)
                return RedirectToAction("Index","Home");

            ViewBag.LoginMessage = result.Message;
            return View("Index");
        }


        [Route("account/logout")]
        public async Task<IActionResult> LogOut()
        {
            
            await _accountApplication.Logout();

            return RedirectToAction("Index", "Home");
        }

        [Route("account/Register")]
        public async Task<IActionResult> Register(RegisterAccount command)
        {
           var result = await _accountApplication.Register(command);
           if (result.IsSucceeded)
               return RedirectToAction("Login");

           ViewBag.RegisterMessage = result.Message;

           return View("Index");
        }

    }
}
