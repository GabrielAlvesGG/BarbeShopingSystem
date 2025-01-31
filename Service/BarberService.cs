using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using MySql.Data.MySqlClient;

namespace BarberShopSystem
{
    public class BarberService
    {
        //public void InsertOrUpdateBarber(Barber Barber)
        //{
        //    try
        //    {
        //        var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //        IConfiguration configuration = builder.Build();
        //        new BarberRepository(configuration).InsertOrUpdateBarber(Barber);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}
        //public List<Barber> ListAllBarber()
        //{
        //    try
        //    {
        //        var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //        IConfiguration configuration = builder.Build();
        //        return new  BarberRepository(configuration).ListAllBarber();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw;
        //    }

        //}
        //public Barber GetBarber(int idOldBarber)
        //{
        //    try
        //    {
        //        var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //        IConfiguration configuration = builder.Build();
        //        return new BarberRepository(configuration).GetBarber(idOldBarber);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}
        public void DeleteBarber(int idBarber)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration configuration = builder.Build();
                new BarberRepository(configuration).DeleteBarber(idBarber); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
