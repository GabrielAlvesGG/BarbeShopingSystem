using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public class LoginService
{
    public bool LoginValidate(string login, string password)
    {
		try
		{
            return LoginRepository.Validade(login, password);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
    }
}
