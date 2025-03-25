using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace BarberShopSystem.ModelsRepository
{
    public class SchedulingRepository : DataBaseRepository
    {
        public SchedulingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<OpeningHours> GetScheduling() 
        {
            try
            {
                List<OpeningHours> openingHours = new List<OpeningHours>();
                var connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM HorariosFuncionamento WHERE BarbeariaId=@BarbeariaId ", connection);
                command.Parameters.AddWithValue("@BarbeariaId", 1); // Precisa ajustar no config para caso tenha mais de uma empresa.
                //command.Parameters.AddWithValue("@DiaSemana", 1); // Aqui também 
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         openingHours.Add(new OpeningHours()
                        {
                            id = reader.GetInt32("Id"),
                            barbeariaId = reader.GetInt32("BarbeariaId"),
                            diaSemana = reader.GetInt32("DiaSemana"),
                            horaAbertura = reader.GetTimeSpan("HoraAbertura"),
                            horaFechamento= reader.GetTimeSpan("HoraFechamento"),
                        });
                    }
                }
                return openingHours;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void BookingATime(Appointments appointments) 
        {
            try
            {   
                var connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Agendamentos (ClienteId, BarbeiroId, ServicoId, DataHorario, Status, BarbeariaId) VALUES (@ClienteId, @BarbeiroId, @ServicoId,@DataHorario, @Status, @BarbeariaId)", connection);
                command.Parameters.AddWithValue("@ClienteId", appointments.client.id);
                command.Parameters.AddWithValue("@BarbeiroId", appointments.barber.id);
                command.Parameters.AddWithValue("@ServicoId", appointments.customer.id);
                command.Parameters.AddWithValue("@DataHorario", appointments.dateTime.ToString($"yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Status", "Pendente");
                command.Parameters.AddWithValue("@BarbeariaId", 1);
                command.ExecuteNonQuery();
                appointments.idAppointments = GetLastInsertedId(connection);
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private int GetLastInsertedId(MySqlConnection connection)
        {
            var getIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
            return Convert.ToInt32(getIdCommand.ExecuteScalar()); // Executando o comando e pegando o ID inserido
        }

        internal bool IsTimeBooked(int? id, DateTime dateTime)
        {
            try
            {

                var connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Agendamentos WHERE ClienteId=@ClienteId AND BarbeariaId=@BarbeariaId AND DataHorario=@DataHorario", connection);
                command.Parameters.AddWithValue("@ClienteId", id);
                command.Parameters.AddWithValue("@DataHorario", dateTime.ToString($"yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@BarbeariaId", 1);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
                connection.Close();

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }
    }
}
