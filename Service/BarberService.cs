using BarberShopSystem.Models;
using MySql.Data.MySqlClient;

namespace BarberShopSystem
{
    public class BarberService
    {
        public void InsertOrUpdateBarber(Barber Barber)
        {
            try
            {
               new BarberRepository().InsertOrUpdateBarber(Barber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public List<Barber> ListAllBarber()
        {
            try
            {
                return new  BarberRepository().ListAllBarber();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
        public Barber GetBarber(int idOldBarber)
        {
            try
            {
                return new BarberRepository().GetBarber(idOldBarber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void DeleteBarber(int idBarber)
        {
            try
            {
                new BarberRepository().DeleteBarber(idBarber); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
