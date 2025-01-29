using BarberShopSystem.Data; // Adicione esta linha, substitua pelo namespace correto
using MySql.Data.MySqlClient;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.ModelsRepository
{
    public class BarberRepository : DataBaseRepository // Corrija o nome do tipo
    {
        public BarberRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Barber> ListAllBarber()
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();
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
                        cpf = reader.GetString("CpfCnpj"),
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
        public void InsertOrUpdateBarber(Barber barber)
        {
            try
            {
                MySqlConnection connection = GetConnection();
                connection.Open();
                string sqlCommand = string.Empty;
                if (barber.Id == 0)
                    sqlCommand = $"Insert into Barber (id, name, email, CpfCnpj, dateOfBirth, password, Phone) VALUES ({barber.Id}, '{barber.Name}', '{barber.Email}','{barber.cpf}', '{barber.DateOfBirth.ToString("yyyy-MM-dd")}', '{barber.PassWord}', '{barber.Phone}')";
                else
                    sqlCommand = $"UPDATE Barber SET name='{barber.Name}', email='{barber.Email}', CpfCnpj='{barber.cpf}', dateOfBirth='{barber.DateOfBirth.ToString("yyyy-MM-dd HH:mm:ss")}', password='{barber.PassWord}', Phone='{barber.Phone}' WHERE Id={barber.Id};";
                                                                                                                                                                  
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
                connection.Open();

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
                        cpf = reader.GetString("CpfCnpj"),
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
                connection.Open();

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
