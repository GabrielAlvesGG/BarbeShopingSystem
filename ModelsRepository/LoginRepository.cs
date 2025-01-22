using BarberShopSystem.Data;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem.ModelsRepository;

public class LoginRepository : DataBaseRepostitory
{
    public static Client GetClient(loginDto login)
    {
        MySqlConnection connection = GetConnection();

        var command = new MySqlCommand($"SELECT * FROM client WHERE ( EMAIL='{login.login}') and PASSWORD='{login.password}'", connection);
        var reader = command.ExecuteReader();

        Client client = null;
        while (reader.Read())
        {
            client = new Client
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Email = reader.GetString("Email"),
                cpf = reader.GetString("CpfCnpj"),
                DateOfBirth = reader.GetDateTime("DateOfBirth"),
                PassWord = reader.GetString("PassWord"),
                Phone = reader.GetString("Phone")
            };
        }
        
            return client;
    }
    
}
