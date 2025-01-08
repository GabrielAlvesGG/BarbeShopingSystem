using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.Models
{
    public class ClientRepository
    {
        public List<Client> ListAllClient()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("server=localhost;database=barbeshopsystem;uid=root;pwd=masterkey;");
                
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM client", connection);
                var reader = command.ExecuteReader();

                List<Client> client = new List<Client>();

                while (reader.Read())
                {
                    client.Add(new Client
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        cpf = reader.GetString("Cpf"),
                        DateOfBird = reader.GetDateTime("DateOfBird"),
                        PassWord = reader.GetString("PassWord"),
                        Phone = reader.GetString("Phone")
                    });
                }
                connection.Close();
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ;
            }
            
        }
    }
}
