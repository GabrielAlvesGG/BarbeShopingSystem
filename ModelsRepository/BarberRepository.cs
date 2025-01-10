using BarberShopSystem.ModelsRepository;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem.Models
{
    public class BarberRepository : DataBaseRepostitory
    {
        public List<Barber> ListAllBarber()
        {
            try
            {
                MySqlConnection connection = GetConnection();

                var command = new MySqlCommand("SELECT * FROM Barber", connection);
                var reader = command.ExecuteReader();

                List<Barber> Barber = new List<Barber>();

                while (reader.Read())
                {
                    Barber.Add(new Barber
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
                return Barber;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ;
            }
            
        }
        public void InsertOrUpdateBarber(Barber Barber)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                string sqlCommand = string.Empty;
                if (Barber.Id == 0)
                    sqlCommand = $"Insert into Barber (id, name, email, cpf, dateOfBirth, password, Phone) VALUES ({Barber.Id}, '{Barber.Name}', '{Barber.Email}','{Barber.cpf}', '{Barber.DateOfBirth.ToString("yyyy-MM-dd")}', '{Barber.PassWord}', '{Barber.Phone}')";
                else
                    sqlCommand = $"UPDATE Barber SET name='{Barber.Name}', email='{Barber.Email}', cpf='{Barber.cpf}', dateOfBirth='{Barber.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss")}', password='{Barber.PassWord}', Phone='{Barber.Phone}' WHERE Id={Barber.Id};";
                                                                                                                                                                  
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
        public Barber GetBarber(int idOldBarber)
        {
            try
            {

                MySqlConnection connection = GetConnection();
                
                var command = new MySqlCommand($"SELECT * FROM Barber WHERE id={idOldBarber}", connection);
                var reader = command.ExecuteReader();

                Barber Barber = new Barber();

                while (reader.Read())
                {
                    Barber = new Barber
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
                return Barber;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void DeleteBarber(int idBarber)
        {
            try
            {
                MySqlConnection connection = GetConnection();

                string sqlCommand = string.Empty;
              
                sqlCommand = $"DELETE FROM Barber WHERE Id={idBarber}";

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
