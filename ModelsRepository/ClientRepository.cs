using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem
{
    public class ClientRepository : DataBaseRepostitory
    {
        public List<Client> ListAllClient()
        {
            try
            {
                MySqlConnection connection = GetConnection();

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
                throw;
            }

        }
        public void InsertClient(Client client)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                string sqlCommand = string.Empty;
                if (client.Id == 0)
                    sqlCommand = $"Insert into Client (id, name, email, cpf, dateOfBirth, password, Phone) VALUES ({client.Id}, '{client.Name}', '{client.Email}','{client.cpf}', '{client.DateOfBirth.ToString("yyyy-MM-dd")}', '{client.PassWord}', '{client.Phone}')";
                else
                    sqlCommand = $"UPDATE CLIENT SET name='{client.Name}', email='{client.Email}', cpf='{client.cpf}', dateOfBirth='{client.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss")}', password='{client.PassWord}', Phone='{client.Phone}' WHERE Id={client.Id};";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public Client GetClient(int idOldClient)
        {
            try
            {
                MySqlConnection connection = GetConnection();

                var command = new MySqlCommand($"SELECT * FROM client WHERE id={idOldClient}", connection);
                var reader = command.ExecuteReader();

                Client client = new Client();

                while (reader.Read())
                {
                    client = new Client
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Email = reader.GetString("Email"),
                        cpf = reader.GetString("Cpf"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        PassWord = reader.GetString("PassWord"),
                        Phone = reader.GetString("Phone")
                    };
                }
                connection.Close();
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void DeleteClient(int idClient)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                string sqlCommand = string.Empty;

                sqlCommand = $"DELETE FROM CLIENT WHERE Id={idClient}";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
