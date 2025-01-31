using BarberShopSystem.Models;
using BarberShopSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarberShopSystem.ModelsRepository;
using BarberShopSyste.Models;

namespace BarberShopSystem.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult RegisterClient()
        {
            return View();
        }
    }
}
