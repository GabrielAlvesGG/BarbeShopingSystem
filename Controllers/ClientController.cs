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
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration configuration = builder.Build();

                new ClientRepository(configuration).DeleteClient(idClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
