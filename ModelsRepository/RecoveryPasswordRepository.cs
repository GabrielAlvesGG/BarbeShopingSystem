using BarberShopSyste.Models;
using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem.ModelsRepository;

public class RecoveryPasswordRepository : DataBaseRepository
{
    public RecoveryPasswordRepository(IConfiguration configuration) : base(configuration) { }

    public bool LoginConfirm(string login)
    {
		try
		{
			return false;
		}
		catch (Exception ex)
		{
            Console.WriteLine(ex.Message);
			throw;
		}
    }

	public void SaveRecoveryRequest(RecoveryPassword recoveryPassword)
	{
		try
		{
            MySqlConnection connection = GetConnection();
            connection.Open();
            string sqlCommand = $"INSERT INTO PasswordResetTokens (Id, Email,Token, Expiration, Used, IdUser) " +
                            $"VALUES ('{recoveryPassword.id}', '{recoveryPassword.email}', '{recoveryPassword.token}','{recoveryPassword.expiration.ToString("yyyy-MM-dd HH:mm:ss")}', {(recoveryPassword.used == false ? 0:1) }, {recoveryPassword.idUser})";

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

    internal bool ValidateToken(string token)
    {
		try
		{
            bool tokenIsTrue = false;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM PasswordResetTokens  WHERE Token=@Token AND Used=0 ", connection);
                command.Parameters.AddWithValue("@Token", token);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tokenIsTrue = true;
                    }
                }
            }
            return tokenIsTrue;
        }
		catch (Exception ex)
		{
            Console.WriteLine(ex.Message);
			throw;
		}
    }
    public Usuario FindUserByResetToken(string tokenUser)
    {
        try
        {
            Usuario user = new Usuario();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT u.Id as Id FROM PasswordResetTokens p INNER JOIN USUARIOS u on u.email=p.email WHERE Token=@Token", connection);
                command.Parameters.AddWithValue("@Token", tokenUser);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Usuario()
                        {
                            id = reader.GetInt32("Id")
                        };
                    }
                }
            }
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    internal void UpdateTokenUsed(string token)
    {
        try
        {
            var connection = GetConnection();
            connection.Open();
            string sqlCommand = $"UPDATE PasswordResetTokens SET Used=1 WHERE Token='{token}'";

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
