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
    internal List<DateTime> AppointmentsMade()
    {
        try
        {
            List<DateTime> appointMentss = new List<DateTime>();
            var connection = GetConnection();
            connection.Open();
            var command = new MySqlCommand("SELECT DataHorario FROM Agendamentos WHERE DataHorario BETWEEN @StartDay AND @EndDay;", connection);
            command.Parameters.AddWithValue("@StartDay", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            command.Parameters.AddWithValue("@EndDay", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    appointMentss.Add(reader.GetDateTime("DataHorario"));
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
            var command = new MySqlCommand("SELECT a.Id as Id, a.DataHorario as DataHorario, s.Descricao as Descricao, s.Preco as Preco FROM Agendamentos a inner join clientes c on c.Id = a.ClienteId inner join servicos s on s.Id = a.ServicoId  WHERE c.UsuarioId=@UsuarioId AND  a.DataHorario > @ComparisonDeadline AND a.status<>'Cancelado'", connection);
            command.Parameters.AddWithValue("@ComparisonDeadline", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            command.Parameters.AddWithValue("@UsuarioId", 12);// O id o usuário é necessário para conseguir filtrar 
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    appointments.Add(
                        new Appointments
                        {
                            idAppointments = reader.GetInt32("Id"),
                            appointments = reader.GetDateTime("DataHorario").TimeOfDay,
                            service = reader.GetString("Descricao"),
                            val = reader.GetDouble("Preco")
                        });
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

    public class Appointments
    {
        public int idAppointments { get; set; }
        public TimeSpan appointments { get; set; }
        public string service { get; set; }
        public double val { get; set; }
    }

}
