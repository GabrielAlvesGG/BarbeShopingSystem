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
            if (SessionHelper.IsUserLoggedIn())
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        public bool Logar([FromBody] loginDto login)
        {
            LoginService loginService = new LoginService();
            Client clientLoggedIn = loginService.LoginValidate(login);
            SessionHelper.UserId = clientLoggedIn.Id;
            SessionHelper.UserName = clientLoggedIn.Name == null ? string.Empty: clientLoggedIn.Name;

            return SessionHelper.IsUserLoggedIn(); 
        }

        public void Logout()
        {
            
            SessionHelper.ClearSession();
        }
        public bool IsUserLoggedIn()
        {
            return SessionHelper.IsUserLoggedIn();
        }
    }
}
