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
                new BarberService().InsertOrUpdateBarber(barber);
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
                return new BarberService().ListAllBarber(); 
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
                return new BarberService().GetBarber(idOldBarber);

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
                new BarberService().DeleteBarber(idBarber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
