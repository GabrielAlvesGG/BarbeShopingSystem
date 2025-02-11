using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;

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
    public Client SearchUserId(int userId)
    {
        Client client = new Client();

        try
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Usuarios WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        client = new Client
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

    public void InsertOrUpdateUser(Client user)
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

    private void InsertUser(Client user, MySqlConnection connection)
    {
        string sqlCommand = $"INSERT INTO Usuarios (Nome, Email, Senha, TipoUsuario, DataCriacao, Telefone) " +
                            $"VALUES ('{user.nome}', '{user.email}', '{user.senha}', '{user.tipoUsuario}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{user.telefone}')";

        var command = new MySqlCommand(sqlCommand, connection);
        command.ExecuteNonQuery(); // Usando ExecuteNonQuery para manipulação de comandos sem retorno de dados.
    }

    private void UpdateUser(Client user, MySqlConnection connection)
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

    public Client GetUser(Client user)
    {
            try
            {

                Client userFound = null;
                bool filtersWhereNecessary = false;

                bool andNecessary = false;

                using (var connection = GetConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM Usuarios";
                    string whereSql = string.Empty;

                    if (user.id != 0)
                    {
                        whereSql += "Id=@Id" + Environment.NewLine;
                        andNecessary = true;
                        filtersWhereNecessary = true;
                    }

                    if (user.nome != string.Empty && user.nome != null)
                    {
                        if (andNecessary)
                        {
                            whereSql += "AND ";
                        }
                        whereSql += "Nome=@Nome" + Environment.NewLine;
                        andNecessary = true;
                        filtersWhereNecessary = true;
                    }

                    if (user.senha != string.Empty && user.senha != null)
                    {
                        if (andNecessary)
                        {
                            whereSql += "AND ";
                        }
                        whereSql += "Senha=@Senha" + Environment.NewLine;
                        andNecessary = true;
                        filtersWhereNecessary = true;
                    }


                    if (user.email != string.Empty && user.email != null)
                    {
                        if (andNecessary)
                        {
                            whereSql += "AND ";
                        }
                        whereSql += "Email=@Email" + Environment.NewLine;
                        andNecessary = true;
                        filtersWhereNecessary = true;
                    }

                    if (user.telefone != string.Empty && user.telefone != null)
                    {
                        if (andNecessary)
                        {
                            whereSql += "AND ";
                        }
                        whereSql += "Telefone=@Telefone" + Environment.NewLine;
                        andNecessary = true;
                        filtersWhereNecessary = true;
                    }
                    if (filtersWhereNecessary)
                        sql += Environment.NewLine + " Where " + Environment.NewLine;

                    sql += whereSql;

                    var command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Id", user.id);
                    command.Parameters.AddWithValue("@Nome", user.nome);
                    command.Parameters.AddWithValue("@Senha", user.senha);
                    command.Parameters.AddWithValue("@Email", user.email);
                    command.Parameters.AddWithValue("@Telefone", user.telefone);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userFound = new Client
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


                return userFound;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        
    }

    internal void UpdatePassword(Client userResetPassword)
    {
        try
        {
            var connection = GetConnection();
            connection.Open();
            string sqlCommand = $"UPDATE Usuarios SET Senha='{userResetPassword.senha}' WHERE Id={userResetPassword.id}";

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

    public List<Client> ListAllBarber()
    {
        try
        {
            MySqlConnection connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand("select * from barbeiros b inner join usuarios u  on u.id = b.usuarioId ;", connection);
            var reader = command.ExecuteReader();

            List<Client> BarberObj = new List<Client>();

            while (reader.Read())
            {
                BarberObj.Add(new Client
                {
                    id = reader.GetInt32("Id"),
                    nome = reader.GetString("Nome"),
                    email = reader.GetString("Email"),
                    telefone = reader.GetString("Telefone"),
                    dataCriacao = reader.GetDateTime("DataCriacao"),
                    barber = new Barber
                    {
                        id = reader.GetInt32("Id"),
                        usuarioId = reader.GetInt32("UsuarioId"),
                        especialidade = reader.GetString("Especialidades"),
                        disponibilidade = reader.GetString("Disponibilidade")
                    }

                });
            }
            connection.Close();
            return BarberObj;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }
}