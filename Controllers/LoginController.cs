using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public bool Logar([FromBody] loginDto login)
        {
            LoginService loginService = new LoginService();
            return loginService.LoginValidate(login.login, login.password);
        }
    }
}
