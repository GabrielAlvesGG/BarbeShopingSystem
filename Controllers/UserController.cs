using BarberShopSyste.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserRegister()
        {
            return View();
        }
        public void InsertUserAndUpdate([FromBody] Usuario user)
        {
            try
            {
                new UserService().InsertOrUpdateUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
