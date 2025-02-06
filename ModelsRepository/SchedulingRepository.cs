using BarberShopSyste.Models;
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

        public OpeningHours GetScheduling() 
        {
            try
            {
                OpeningHours openingHours = new OpeningHours();
                var connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM HorariosFuncionamento WHERE BarbeariaId=@BarbeariaId and DiaSemana=@DiaSemana ", connection);
                command.Parameters.AddWithValue("@BarbeariaId", 1);
                command.Parameters.AddWithValue("@DiaSemana", 1);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        openingHours = new OpeningHours
                        {
                            id = reader.GetInt32("Id"),
                            barbeariaId = reader.GetInt32("BarbeariaId"),
                            diaSemana = reader.GetInt32("DiaSemana"),
                            horaAbertura = reader.GetTimeSpan("HoraAbertura"),
                            horaFechamento= reader.GetTimeSpan("HoraFechamento"),
                        };
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

        public void BookingATime(Appointments appointments) // Ajustar esse insert
        {
            try
            {
                var connection = GetConnection();
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Agendamentos (ClienteId, BarbeiroId, ServicoId, DataHorario, Status) VALUES (@ClienteId, @BarbeiroId, @ServicoId,@DataHorario, @Status)", connection);
                command.Parameters.AddWithValue("@ClienteId", appointments.client.id);
                command.Parameters.AddWithValue("@BarbeiroId", 1);
                command.Parameters.AddWithValue("@ServicoId", appointments.customer.id);
                command.Parameters.AddWithValue("@DataHorario", appointments.dateTime.ToString($"yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Status", "Confirmado");
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        //public List<TimeSpan> GerarHorariosDeUmaEmUmaHora(int barbeariaId, DateTime data)
        //{
        //    using (var db = new MeuDbContext())
        //    {
        //        var horario = db.HorariosFuncionamento
        //            .FirstOrDefault(h => h.BarbeariaId == barbeariaId && h.DiaSemana == (int)data.DayOfWeek);

        //        if (horario == null) return new List<TimeSpan>(); 

        //        var horariosDisponiveis = new List<TimeSpan>();
        //        var horaAtual = horario.HoraAbertura;

        //        while (horaAtual < horario.HoraFechamento)
        //        {
        //            horariosDisponiveis.Add(horaAtual);
        //            horaAtual = horaAtual.Add(TimeSpan.FromHours(1));
        //        }

        //        return horariosDisponiveis;
        //    }
        //}

    }
}
