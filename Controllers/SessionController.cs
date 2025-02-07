using BarberShopSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class SessionController : Controller
    {
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
