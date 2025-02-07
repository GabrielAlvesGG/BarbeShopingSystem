using BarberShopSystem.Models;
using BarberShopSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarberShopSystem.ModelsRepository;

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
