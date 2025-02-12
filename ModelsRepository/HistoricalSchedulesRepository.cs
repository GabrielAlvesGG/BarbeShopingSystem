using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.ModelsRepository;

public class HistoricalSchedulesRepository : DataBaseRepository
{
    public HistoricalSchedulesRepository(IConfiguration configuration) : base(configuration)
    {
    }
    internal void RegisterConfirmeOrCancelAppointments(int idAppointment,Appointments appointments, StatusAppointmentEnum newStatus)
    {
        try
        {
            MySqlConnection connection = GetConnection();
            connection.Open();
            string sqlCommand = string.Empty;

            sqlCommand = $"INSERT INTO AgendamentoHistorico (AgendamentoId, StatusAntigo, StatusNovo, Acao, UsuarioId, DataAcao)\r\nVALUES (@AgendamentoId, @StatusAntigo, @StatusNovo, @Acao, @UsuarioId, @DateTimeNow)";
            var command = new MySqlCommand(sqlCommand, connection);

            string action = newStatus == StatusAppointmentEnum.Cancelado ? "O Agendamento foi cancelado" : StatusAppointmentEnum.Confirmado == newStatus ? "Agendamento confirmado" : StatusAppointmentEnum.Pendente == newStatus ? "Novo agendamento, Aguardando confirmação!" : StatusAppointmentEnum.SemStatus == newStatus ? "Status não identificado" : "Status não identificado";

            StatusAppointmentEnum oldStatus = StatusAppointmentEnum.SemStatus;
            if (Enum.TryParse(appointments.statusAppointment, out StatusAppointmentEnum statusConvertido))
            {
                oldStatus = statusConvertido;
            }

            command.Parameters.AddWithValue("@AgendamentoId", idAppointment);
            command.Parameters.AddWithValue("@StatusAntigo", oldStatus);
            command.Parameters.AddWithValue("@StatusNovo", newStatus);
            command.Parameters.AddWithValue("@Acao", action);
            command.Parameters.AddWithValue("@UsuarioId", SessionHelper.UserId);
            command.Parameters.AddWithValue("@DateTimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));



            command.ExecuteReader();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
