using BarberShopSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult RegisterClient()
        {
            return View();
        }
        
        public void RegisterNewClient([FromBody] Client client)
        {
            ClientRepository clientRepository = new ClientRepository();
            clientRepository.ListAllClient();
        }
    }
}
