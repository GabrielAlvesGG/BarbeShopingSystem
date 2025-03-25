using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data.SqlClient;
using System.Globalization;

namespace BarberShopSystem.ModelsRepository
{
    public class ServicesProvidedRepository : DataBaseRepository
    {
        public ServicesProvidedRepository(IConfiguration configuration) : base(configuration)
        {
        }

     


        public void InsertServicesProvided(ServicesProvided servicesProvided)
        {
            try
            {

                MySqlConnection connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand($"INSERT INTO servicos (Nome, Descricao, preco,duracao) " +
                                $"VALUES (@Name, @Description, @Price,@duration)", connection);
                command.Parameters.AddWithValue("@Name", servicesProvided.name);
                command.Parameters.AddWithValue("@Description", servicesProvided.description);
                string priceString = servicesProvided.price.ToString().Replace(",", ".");
                double price = Convert.ToDouble(priceString, CultureInfo.InvariantCulture);

                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@duration", servicesProvided.duration);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void UpdateServicesProvided(ServicesProvided servicesProvided)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();
                string sqlCommand = $"UPDATE servicos SET Nome='{servicesProvided.name}', Descricao='{servicesProvided.description}', preco='{servicesProvided.price.ToString().Replace(",", ".")}', duracao={servicesProvided.duration} WHERE Id={servicesProvided.id}";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public int GetLastInsertedId(MySqlConnection connection)
        {
            try
            {
                var getIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
                return Convert.ToInt32(getIdCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }

        internal List<ServicesProvided> AllServices()
        {
            try
            {
                List<ServicesProvided> ServicesProvideds = new List<ServicesProvided>();
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM Servicos", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServicesProvideds.Add(new ServicesProvided
                            {
                                id = reader.GetInt32("Id"),
                                name = reader.GetString("Nome"),
                                description = reader.GetString("Descricao"),
                                price = reader.GetDouble("Preco").ToString(),
                                duration = reader.GetInt32("duracao")
                            });
                        }
                    }
                    connection.Close();

                }

                return ServicesProvideds;




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal ServicesProvided GetServicesId(int idServices)
        {
            try
            {
                ServicesProvided ServicesProvided = new ServicesProvided();
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM Servicos WHERE Id=@Id", connection);
                    command.Parameters.AddWithValue("@Id", idServices);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            ServicesProvided.id = reader.GetInt32("Id");
                            ServicesProvided.name = reader.GetString("Nome");
                            ServicesProvided.description = reader.GetString("Descricao");
                            ServicesProvided.price = reader.GetDouble("Preco").ToString();
                            ServicesProvided.duration= reader.GetInt32("duracao");

                        }
                    }
                    connection.Close();

                }

                return ServicesProvided;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
    }

        internal void DeleteServices(int idServices)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();
                string sqlCommand = $"DELETE FROM SERVICOS WHERE Id={idServices}";

                var command = new MySqlCommand(sqlCommand, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
    }
