using BarberShopSystem.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem.ModelsRepository;

public class LoginRepository : DataBaseRepostitory
{
    public static bool Validade(string login, string password)
    {
        MySqlConnection connection = GetConnection();

        var command = new MySqlCommand($"SELECT * FROM client WHERE ( EMAIL='{login}') and PASSWORD='{password}'", connection);
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
        if (client != null)
            return true;
        else
            return false;
    }
    
}
