using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.ModelsRepository;

public class AppointmentsRepository : DataBaseRepository
{
    public AppointmentsRepository(IConfiguration configuration) : base(configuration)
    {
    }
    internal List<Appointments> AppointmentsMade()
    {
        try
        {
            List<Appointments> appointMentss = new List<Appointments>();
            var connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand("SELECT DataHorario, BarbeiroId FROM Agendamentos WHERE DataHorario BETWEEN @StartDay AND @EndDay AND Status <> 'Cancelado';", connection);
            command.Parameters.AddWithValue("@StartDay", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@EndDay", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    appointMentss.Add(new Appointments { 
                     dateTime =  reader.GetDateTime("DataHorario"),
                     barber = new Barber
                     {
                         id = reader.GetInt32("BarbeiroId")
                     },
                    });
                }
            }
            connection.Close();
            return appointMentss;


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }


    public List<Appointments> HasClientScheduling(int userId)
    {
        try
        {
            List<Appointments> appointments = new List<Appointments>();


            var connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand("SELECT a.Id as Id, a.DataHorario as DataHorario,a.Status as Status, s.Descricao as Descricao, s.Preco as Preco FROM Agendamentos a inner join clientes c on c.Id = a.ClienteId inner join servicos s on s.Id = a.ServicoId  WHERE c.UsuarioId=@UsuarioId AND  a.DataHorario > @ComparisonDeadline AND a.status<>'Cancelado'", connection);
            command.Parameters.AddWithValue("@ComparisonDeadline", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@UsuarioId", userId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointments appointment = new Appointments();

                    appointment.idAppointments = reader.GetInt32("Id");
                    appointment.dateTime = reader.GetDateTime("DataHorario");
                    appointment.customer.description = reader.GetString("Descricao");
                    appointment.customer.price = reader.GetDouble("Preco");
                    appointment.statusAppointment = reader.GetString("Status");
                    appointments.Add(appointment);

                }
            }
            connection.Close();

            return appointments;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }


    }

    internal bool CancelAppointment(int idAppointment)
    {
        try
        {

            MySqlConnection connection = GetConnection();
            connection.Open();
            string sqlCommand = string.Empty;

            sqlCommand = $"UPDATE Agendamentos SET Status='{StatusAppointmentEnum.Cancelado}' WHERE Id={idAppointment};";
            var command = new MySqlCommand(sqlCommand, connection);

            command.ExecuteReader();
            connection.Close();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    

}
