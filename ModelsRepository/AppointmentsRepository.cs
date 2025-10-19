using BarberShopSystem.Data;
using BarberShopSystem.Enums;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;
using System.Data;

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
            var appointments = new List<Appointments>();
            using var connection = GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                "SELECT DataHorario, BarbeiroId FROM Agendamentos WHERE DataHorario > @StartDay AND Status <> 'Cancelado';",
                connection
            );
            command.Parameters.Add("@StartDay", MySqlDbType.DateTime).Value = DateTime.Now;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                appointments.Add(new Appointments
                {
                    dateTime = reader.GetDateTime("DataHorario"),
                    barber = new Barber
                    {
                        id = reader.GetInt32("BarbeiroId")
                    },
                });
            }

            return appointments;
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

            using var connection = GetConnection();
            connection.Open();

            string query = "SELECT " + Environment.NewLine;
            query += "a.Id as Id," + Environment.NewLine;
            query += "a.DataHorario as DataHorario," + Environment.NewLine;
            query += "a.Status as Status," + Environment.NewLine;
            query += "s.Descricao as Descricao," + Environment.NewLine;
            query += "s.Duracao as Duracao," + Environment.NewLine;
            query += "s.Preco as Preco," + Environment.NewLine;
            query += "uc.Nome AS NomeCliente," + Environment.NewLine;
            query += "ub.Nome AS NomeBarbeiro," + Environment.NewLine;
            query += "CASE " + Environment.NewLine;
            query += "WHEN @UsuarioId = b.UsuarioId THEN uc.Nome" + Environment.NewLine; // Se o usuário logado for o barbeiro, exibe o nome do cliente
            query += "WHEN @UsuarioId = c.UsuarioId THEN ub.Nome" + Environment.NewLine; // Se o usuário logado for o cliente, exibe o nome do Barbeiro
            query += "ELSE NULL" + Environment.NewLine;
            query += "END AS NomeExibido" + Environment.NewLine;
            query += "FROM " + Environment.NewLine;
            query += "Agendamentos a" + Environment.NewLine;
            query += "LEFT JOIN" + Environment.NewLine;
            query += "Clientes c" + Environment.NewLine;
            query += "ON" + Environment.NewLine;
            query += "a.ClienteId = c.Id" + Environment.NewLine;
            query += "INNER JOIN" + Environment.NewLine;
            query += "Servicos s" + Environment.NewLine;
            query += "ON" + Environment.NewLine;
            query += "s.Id = a.ServicoId" + Environment.NewLine;
            query += "INNER JOIN" + Environment.NewLine;
            query += "Barbeiros b" + Environment.NewLine;
            query += "ON" + Environment.NewLine;
            query += "a.BarbeiroId= b.Id" + Environment.NewLine;
            query += "LEFT JOIN" + Environment.NewLine;
            query += "Usuarios uc" + Environment.NewLine;
            query += "ON" + Environment.NewLine;
            query += "uc.Id = c.UsuarioId" + Environment.NewLine;
            query += "INNER JOIN " + Environment.NewLine;
            query += "Usuarios ub" + Environment.NewLine;
            query += "ON" + Environment.NewLine;
            query += "ub.Id = b.UsuarioId" + Environment.NewLine;
            query += "WHERE" + Environment.NewLine;
            query += "a.DataHorario > @ComparisonDeadline" + Environment.NewLine;
            query += "AND" + Environment.NewLine;
            query += "a.status<>'Cancelado'" + Environment.NewLine;
            query += "AND" + Environment.NewLine;
            query += " ((@UsuarioId = b.UsuarioId AND (uc.Nome IS NOT NULL OR uc.Nome IS NULL)) " + Environment.NewLine;
            query += " OR" + Environment.NewLine;
            query += "(@UsuarioId = c.UsuarioId AND ub.Nome IS NOT NULL)" + Environment.NewLine;
            query += ")" + Environment.NewLine;

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@ComparisonDeadline", MySqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@UsuarioId", MySqlDbType.Int32).Value = userId;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointments appointment = new Appointments();

                    // Ensure nested objects exist before setting properties
                    appointment.customer ??= new();
                    appointment.client ??= new();
                    appointment.barber ??= new();

                    appointment.idAppointments = reader.GetInt32("Id");
                    appointment.dateTime = reader.GetDateTime("DataHorario");
                    appointment.customer.description = reader.GetString("Descricao");
                    appointment.customer.price = reader.GetDouble("Preco");
                    appointment.customer.duration = reader.GetInt32("Duracao");
                    appointment.statusAppointment = reader.GetString("Status");

                    int nomeExibidoOrdinal = reader.GetOrdinal("NomeExibido");
                    appointment.nameShowBarberOrCliente = reader.IsDBNull(nomeExibidoOrdinal)
                        ? "Ocupado"
                        : reader.GetString(nomeExibidoOrdinal);

                    appointment.showNameBarber = appointment.nameShowBarberOrCliente == reader.GetString("NomeBarbeiro");
                    appointments.Add(appointment);
                }
            }

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
            using var connection = GetConnection();
            connection.Open();
            const string sqlCommand = "UPDATE Agendamentos SET Status = @Status WHERE Id = @Id;";
            using var command = new MySqlCommand(sqlCommand, connection);
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = StatusAppointmentEnum.Cancelado.ToString();
            command.Parameters.Add("@Id", MySqlDbType.Int32).Value = idAppointment;

            int affected = command.ExecuteNonQuery();
            return affected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    internal void ConfirmeAppointments(int idAppointment)
    {
        try
        {
            using var connection = GetConnection();
            connection.Open();
            const string sqlCommand = "UPDATE Agendamentos SET Status = @Status WHERE Id = @Id;";
            using var command = new MySqlCommand(sqlCommand, connection);
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = StatusAppointmentEnum.Confirmado.ToString();
            command.Parameters.Add("@Id", MySqlDbType.Int32).Value = idAppointment;

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    internal Appointments GetAppointmentsId(int idAppointment)
    {
        try
        {
            Appointments appointment = new Appointments();

            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT " + Environment.NewLine
                         + " * " + Environment.NewLine
                         + "FROM Agendamentos " + Environment.NewLine
                         + "WHERE id=@IdAppointment; " + Environment.NewLine;

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@IdAppointment", MySqlDbType.Int32).Value = idAppointment;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Ensure nested objects exist before setting properties
                appointment.client ??= new();
                appointment.barber ??= new();

                appointment.idAppointments = reader.GetInt32("Id");
                appointment.client.id = reader.IsDBNull(reader.GetOrdinal("ClienteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ClienteId"));
                appointment.barber.id = reader.GetInt32("BarbeiroId");
                appointment.dateTime = reader.GetDateTime("DataHorario");
                appointment.statusAppointment = reader.GetString("Status");
            }

            return appointment;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
