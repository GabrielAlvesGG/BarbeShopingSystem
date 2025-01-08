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
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
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

        public void RegisterNewClient(Client client)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("server=localhost;database=barbeshopsystem;uid=root;pwd=masterkey;");
                connection.Open();
                var command = new MySqlCommand($"Insert into Client (id, name, email, cpf, dateOfBirth, password, Phone) VALUES ({client.Id}, '{client.Name}', '{client.Email}','{client.cpf}', '{client.DateOfBirth.ToString("yyyy-MM-dd")}', '{client.PassWord}', '{client.Phone}')", connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
