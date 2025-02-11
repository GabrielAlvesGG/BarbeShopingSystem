using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public class UserService
{
    private readonly UserRepository _userRepository;
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void InsertOrUpdateUser(Client user)
    {
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            new UserRepository(configuration).InsertOrUpdateUser(user);
            
            if (user.tipoUsuario == TipoUsuarioEnum.Barbeiro)
            {
                if (user.barber == null)
                    user.barber = new Barber();

                user.barber.usuarioId = user.id;
                new BarberRepository(configuration).InsertOrUpdateBarber(user.barber);
            }
            if (user.tipoUsuario == TipoUsuarioEnum.Cliente)
            {   
                if(user.client == null)
                    user.client = new Cliente();

                user.client.usuarioId = user.id;
                new ClientRepository(configuration).InsertOrUpdateClient(user.client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public Client GetUser(Client user)
    {
        try
        {
            return _userRepository.GetUser(user); 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public List<Client> GetAllBarbers()
    {
        try
        {
            return _userRepository.ListAllBarber();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
