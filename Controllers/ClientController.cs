using BarberShopSystem.Models;
using BarberShopSystem;
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
                new ClientService().InsertClient(client);
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
                return new ClientService().ListAllClient(); ;
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
               return new ClientService().GetClient(idOldClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void DeleteClient([FromBody] int idClient)
        {
            try
            {
                new ClientRepository().DeleteClient(idClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
