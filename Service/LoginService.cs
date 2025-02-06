using BarberShopSyste.Models;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using System.Configuration;

namespace BarberShopSystem.Service;

public class LoginService
{

    public Client LoginValidate(loginDto login)
    {
        try
        {
            var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            LoginRepository loginRepository = new LoginRepository(configuration);
            return  loginRepository.LoginValidate(login); ;

        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
    }
  
}
