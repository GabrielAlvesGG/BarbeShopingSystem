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

    public async Task InsertOrUpdateUserAsync(User user, IFormFile file)
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
            
                if (file != null)
                {
                    try
                    {
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Barbers");
                        Directory.CreateDirectory(uploadPath); // Cria a pasta, se não existir

                        string fileName = $"{Guid.NewGuid()}_{file.FileName}"; // Evita nomes duplicados
                        string filePath = Path.Combine(uploadPath, fileName);

                        Console.WriteLine($"Salvando arquivo: {filePath}");

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            Console.WriteLine("Arquivo salvo com sucesso!");
                        }

                        user.barber.imgUrl = "/Images/Barbers/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
                        throw;
                    }
                }
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

    public User GetUser(User user)
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

    public List<User> GetAllBarbers()
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
