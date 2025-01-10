using BarberShopSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void InsertClient([FromBody] Client client)
        {
            try
            {
                ClientRepository clientRepository = new ClientRepository();
                clientRepository.InsertClient(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
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

        public Client EditClient([FromBody] int idOldClient)
        {
            try
            {
               ClientRepository clientRepository = new ClientRepository();

               return clientRepository.GetClient(idOldClient);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
