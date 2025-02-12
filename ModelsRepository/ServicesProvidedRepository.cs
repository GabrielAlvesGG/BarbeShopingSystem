using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace BarberShopSystem.ModelsRepository
{
    public class ServicesProvidedRepository : DataBaseRepository
    {
        public ServicesProvidedRepository(IConfiguration configuration) : base(configuration)
        {
        }

        internal void InsertOrUpdateProvided(ServicesProvided serviceProvided)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();

                if (serviceProvided.id == 0)
                {
                    InsertServicesProvided(serviceProvided, connection);
                    serviceProvided.id = GetLastInsertedId(connection);
                }
                else
                    UpdateServicesProvided(serviceProvided, connection);
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        private void InsertServicesProvided(ServicesProvided servicesProvided, MySqlConnection connection)
        {
            string sqlCommand = $"INSERT INTO servicos (Nome, Descricao, preco) " +
                                $"VALUES ('{servicesProvided.name}', '{servicesProvided.description}', '{Convert.ToDouble(servicesProvided.price, new CultureInfo("pt-BR"))}')";

            var command = new MySqlCommand(sqlCommand, connection);
            command.ExecuteNonQuery(); 
        }

        private void UpdateServicesProvided(ServicesProvided servicesProvided, MySqlConnection connection)
        {
            string sqlCommand = $"UPDATE servicos SET Nome='{servicesProvided.name}', Descricao='{servicesProvided.description}', preco='{Convert.ToDouble(servicesProvided.price, new CultureInfo("pt-BR"))}' WHERE Id={servicesProvided.id}";

            var command = new MySqlCommand(sqlCommand, connection);
            command.ExecuteNonQuery(); 
        }

        private int GetLastInsertedId(MySqlConnection connection)
        {
            var getIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
            return Convert.ToInt32(getIdCommand.ExecuteScalar()); 
        }
    }
}
