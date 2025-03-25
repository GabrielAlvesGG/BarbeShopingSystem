using BarberShopSystem.Data;
using MySql.Data.MySqlClient;
using BarberShopSystem.Models;
using System.Data;
using BarberShopSystem.Enums;


namespace BarberShopSystem.ModelsRepository
{

    public class LoginRepository : DataBaseRepository
    {
        public LoginRepository(IConfiguration configuration) : base(configuration) { }

        public User LoginValidate(loginDto login)
        {
            User client = new User();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT Id, Nome, TipoUsuario FROM Usuarios WHERE Email=@Email  and Senha=@Senha", connection);
                    command.Parameters.AddWithValue("@Email", login.login);
                    command.Parameters.AddWithValue("@Senha", login.password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new User
                            {
                                id = reader.GetInt32("Id"),
                                nome = reader.GetString("Nome"),
                                tipoUsuario = reader.GetString("TipoUsuario") == "Administrador" ? TipoUsuarioEnum.Administrador : reader.GetString("TipoUsuario") == "Cliente" ? TipoUsuarioEnum.Cliente : reader.GetString("TipoUsuario") == "Barbeiro" ? TipoUsuarioEnum.Barbeiro : TipoUsuarioEnum.Anonimo
                            };
                        }
                    }
                }
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}