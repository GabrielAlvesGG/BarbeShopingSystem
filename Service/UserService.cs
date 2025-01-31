using BarberShopSyste.Models;
using BarberShopSystem.Enums;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public class UserService
{
    public void InsertOrUpdateUser(Usuario user)
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
                user.barber.usuarioId = user.id;
                new BarberRepository(configuration).InsertOrUpdateBarber(user.barber);
            }
            if (user.tipoUsuario == TipoUsuarioEnum.Cliente)
            {
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
}
