using BarberShopSystem.Models;
using BarberShopSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberShopSystem.Controllers
{
    public class BarberController : Controller
    {
        public IActionResult RegisterBarber()
        {
            return View();
        }
        public IActionResult ListBarber()
        {
            return View();
        }

        public void InsertBarber([FromBody] Barber barber)
        {
            try
            {
                BarberRepository barberRepository = new BarberRepository();
                barberRepository.InsertBarber(barber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<Barber> ListAllBarbers()
        {
            try
            {
                BarberRepository barberRepository = new BarberRepository();
                List<Barber> allBarbers = barberRepository.ListAllBarber();
                return allBarbers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public Barber EditBarber([FromBody] int idOldBarber)
        {
            try
            {
               BarberRepository barberRepository = new BarberRepository();

               return barberRepository.GetBarber(idOldBarber);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void DeleteBarber([FromBody] int idBarber)
        {
            try
            {
                BarberRepository barberRepository = new BarberRepository();
                barberRepository.DeleteBarber(idBarber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
