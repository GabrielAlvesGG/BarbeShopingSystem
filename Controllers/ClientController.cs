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
        public IActionResult ListClients()
        {
            return View();
        }

        public void RegisterNewClient([FromBody] Client client)
        {
            
        }
        public List<Client> ListAllClients()
        {
            try
            {
                ClientRepository clientRepository = new ClientRepository();
                List<Client> allClients = clientRepository.ListAllClient();
                return allClients;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
