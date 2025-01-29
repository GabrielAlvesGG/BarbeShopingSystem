using BarberShopSystem.Data;
using MySql.Data.MySqlClient;
using BarberShopSystem.Models;
using System.Data;


namespace BarberShopSystem.ModelsRepository
{

    public class LoginRepository : DataBaseRepository
    {
        public LoginRepository(IConfiguration configuration) : base(configuration) { }

        public Client GetClient(loginDto login) // Corrija o nome do tipo
        {
            Client client = new Client();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM client WHERE EMAIL=@Email AND PASSWORD=@Password", connection);
                    command.Parameters.AddWithValue("@Email", login.login);
                    command.Parameters.AddWithValue("@Password", login.password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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