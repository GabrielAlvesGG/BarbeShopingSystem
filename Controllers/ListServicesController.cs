using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
