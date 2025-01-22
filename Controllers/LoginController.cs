using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            LoginService loginService = new LoginService();
            if (loginService.IsUserLoggedIn())
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        public bool Logar([FromBody] loginDto login)
        {
            LoginService loginService = new LoginService();
            Client clientLoggedIn = loginService.LoginValidate(login);
            SessionHelper.UserId = clientLoggedIn.Id;
            SessionHelper.UserName = clientLoggedIn.Name;

            return loginService.IsUserLoggedIn(); 
        }
    }
}
