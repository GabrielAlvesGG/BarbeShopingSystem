using Microsoft.AspNetCore.Mvc;
using BarberShopSystem.Helpers;

namespace BarberShopSystem.Controllers
{
    public class BarberController : Controller
    {
        public IActionResult RegisterBarber()
        {
            if (SessionHelper.IsUserLoggedIn() && SessionHelper.IsMasterUser())
                return RedirectToAction("Login", "Login");
            
            return View();
        }
        public IActionResult ListBarber()
        {
            return View();
        }

        public void DeleteBarber([FromBody] int idBarber)
        {
            try
            {
                new BarberService().DeleteBarber(idBarber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
