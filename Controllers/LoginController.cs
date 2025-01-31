using BarberShopSyste.Models;
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

        public IActionResult RecoverPassword()
        {
            if (SessionHelper.IsUserLoggedIn())
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        public bool Logar([FromBody] loginDto login)
        {
            LoginService loginService = new LoginService();
            Usuario clientLoggedIn = loginService.LoginValidate(login);
            SessionHelper.UserId = clientLoggedIn.id;
            SessionHelper.UserName = clientLoggedIn.nome == null ? string.Empty: clientLoggedIn.nome;
            SessionHelper.UserType = clientLoggedIn.tipoUsuario.ToString();

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
        public bool IsUserMaster()
        {
            try
            {
                return SessionHelper.IsMasterUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
