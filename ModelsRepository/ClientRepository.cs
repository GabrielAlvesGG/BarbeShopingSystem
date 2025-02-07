using BarberShopSystem.Data; 
using BarberShopSystem.Models;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using BarberShopSystem.ModelsRepository;
using System.Threading;
using BarberShopSystem.Enums;

namespace BarberShopSystem.ModelsRepository
{
    public class ClientRepository : DataBaseRepository // Corrija o nome do tipo
    {
        public ClientRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public void InsertOrUpdateClient(Cliente client)
        {
            try
            {

                MySqlConnection connection = GetConnection();
                connection.Open();
                string sqlCommand = string.Empty;
                if (client.id == 0)
                    sqlCommand = $"Insert into Clientes ( UsuarioId, Endereco) VALUES ( '{client.usuarioId}', '{client.endereco}')";
                else
                    sqlCommand = $"UPDATE Clientes SET UsuarioId='{client.usuarioId}', Endereco='{client.endereco}' WHERE Id={client.id};";
                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public Cliente SearchClientWichUserId(int userId)
        {
            Cliente client = new Cliente();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM clientes WHERE UsuarioId=@Id", connection);
                    command.Parameters.AddWithValue("@Id", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new Cliente
                            {
                                id = reader.GetInt32("Id"),
                                endereco = reader.GetString("Endereco"),
                                usuarioId = reader.GetInt32("UsuarioId"),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return client;
        }

    }
}
