using BarberShopSyste.Models;
using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.ModelsRepository;

public class UserRepository : DataBaseRepository
{
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        //public void InsertOrUpdateUser(Usuario user)
        //{
        //    try
        //    {
        //        MySqlConnection connection = GetConnection();
        //        connection.Open();
        //        string sqlCommand = string.Empty;
        //        if (user.id == 0)
        //            sqlCommand = $"Insert into Usuarios (Nome, Email, Senha, TipoUsuario,DataCriacao, telefone) VALUES ( '{user.nome}', '{user.email}','{user.senha}', '{user.tipoUsuario}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{user.telefone}')";
        //        else
        //            sqlCommand = $"UPDATE Usuarios SET Name='{user.nome}', Email='{user.email}',Senha='{user.senha}', TipoUsuario='{user.tipoUsuario}', Telefone='{user.telefone}' WHERE Id={user.id};";

        //        var command = new MySqlCommand(sqlCommand, connection);
        //        command.ExecuteReader();
        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}
        public Usuario GetUserId(Usuario user)
        {
            Usuario client = new Usuario();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM Usuarios WHERE Id=@Id", connection);
                    command.Parameters.AddWithValue("@Id", user.id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new Usuario
                            {
                                id = reader.GetInt32("Id"),
                                nome = reader.GetString("Nome"),
                                email = reader.GetString("Email"),
                                dataCriacao = reader.GetDateTime("DataCriacao"),
                                senha = reader.GetString("Senha"),
                                telefone = reader.GetString("Telefone"),
                                tipoUsuario = reader.GetString("TipoUsuario") == "Administrador" ? TipoUsuarioEnum.Administrador : reader.GetString("TipoUsuario") == "cliente" ? TipoUsuarioEnum.Cliente : reader.GetString("TipoUsuario") == "barbeiro" ? TipoUsuarioEnum.Barbeiro : TipoUsuarioEnum.Anonimo
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

    public void InsertOrUpdateUser(Usuario user)
    {
        try
        {
            MySqlConnection connection = GetConnection();
            connection.Open();

            if (user.id == 0)
            {
                // Comando de INSERT
                InsertUser(user, connection);
                user.id = GetLastInsertedId(connection); 
            }
            else
            {
                // Comando de UPDATE
                UpdateUser(user, connection);
            }

            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    private void InsertUser(Usuario user, MySqlConnection connection)
    {
        string sqlCommand = $"INSERT INTO Usuarios (Nome, Email, Senha, TipoUsuario, DataCriacao, Telefone) " +
                            $"VALUES ('{user.nome}', '{user.email}', '{user.senha}', '{user.tipoUsuario}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{user.telefone}')";

        var command = new MySqlCommand(sqlCommand, connection);
        command.ExecuteNonQuery(); // Usando ExecuteNonQuery para manipulação de comandos sem retorno de dados.
    }

    private void UpdateUser(Usuario user, MySqlConnection connection)
    {
        string sqlCommand = $"UPDATE Usuarios SET Nome='{user.nome}', Email='{user.email}', Senha='{user.senha}', TipoUsuario='{user.tipoUsuario}', Telefone='{user.telefone}' WHERE Id={user.id}";

        var command = new MySqlCommand(sqlCommand, connection);
        command.ExecuteNonQuery(); // Usando ExecuteNonQuery para manipulação de comandos sem retorno de dados.
    }

    private int GetLastInsertedId(MySqlConnection connection)
    {
        var getIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
        return Convert.ToInt32(getIdCommand.ExecuteScalar()); // Executando o comando e pegando o ID inserido
    }

}
