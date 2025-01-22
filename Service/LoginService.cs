using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public class LoginService
{
    public Client LoginValidate(loginDto login)
    {
		try
		{
            return LoginRepository.GetClient(login); ;

        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
    }
    public bool IsUserLoggedIn()
    {
        return SessionHelper.UserId != 0 && SessionHelper.UserId != null;
    }
}
