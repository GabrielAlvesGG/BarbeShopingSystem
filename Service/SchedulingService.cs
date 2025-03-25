using BarberShopSystem.Enums;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BarberShopSystem.Service;

public class SchedulingService
{
	private readonly SchedulingRepository _schedulingRepository;
	private readonly AppointmentsRepository _appointmentsRepository;
	private readonly UserRepository _userRepository;
    private readonly ClientRepository _clientRepository;
    private readonly HistoricalSchedulesRepository _historicalSchedulesRepository;

	public SchedulingService(SchedulingRepository schedulingRepository, AppointmentsRepository appointmentsRepository,UserRepository userRepository, ClientRepository clientRepository, HistoricalSchedulesRepository historicalSchedulesRepository)
    {
        _schedulingRepository = schedulingRepository;
        _appointmentsRepository = appointmentsRepository;
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        _historicalSchedulesRepository = historicalSchedulesRepository;
    }
    public string GetScheduling()
     {
        try
        {
            List<User> usersBarber = new List<User>();

            if (SessionHelper.IsBarberUser())
                usersBarber = _userRepository.ListBarberId(SessionHelper.UserId);
            else
                usersBarber = _userRepository.ListAllBarber();

            List<WeekDays> weekDays = ObterProximosSeteDias();

            List<OpeningHours> openingHours = _schedulingRepository.GetScheduling();
            List<Schedules> schedules = new List<Schedules>();

            foreach (var day in weekDays)
            {
                TimeSpan horaAbertura = new TimeSpan();
                TimeSpan horaFechamento = new TimeSpan();
                    for (int i = 0; i < openingHours.Count; i++)
                    {
                        if (day.dayString == "segunda-feira")
                        if (openingHours[i].diaSemana == 1)
                            {
                            horaAbertura = openingHours[i].horaAbertura;
                            horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }
                            
                        if (day.dayString == "terça-feira")
                            if (openingHours[i].diaSemana == 2)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }
                        if (day.dayString == "quarta-feira")
                            if (openingHours[i].diaSemana == 3)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }
                        if (day.dayString == "quinta-feira")
                            if (openingHours[i].diaSemana == 4)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }
                        if (day.dayString == "sexta-feira")
                            if (openingHours[i].diaSemana == 5)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                                break;
                            }
                        if (day.dayString == "sábado")
                            if (openingHours[i].diaSemana == 6)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }
                        if (day.dayString == "domingo")
                            if (openingHours[i].diaSemana == 7)
                            {
                                horaAbertura = openingHours[i].horaAbertura;
                                horaFechamento = openingHours[i].horaFechamento;
                            break;
                        }

                    }
                foreach (var item in GerarHorarios(horaAbertura, horaFechamento))
                {
                    bool freeAppointiments = false;
                    if (day.dayDateTime.Date == DateTime.Now.Date)
                        freeAppointiments = item > DateTime.Now.TimeOfDay;
                    else
                        freeAppointiments = true;


                    day.schedules.Add(new Schedules()
                    {
                        free = freeAppointiments,
                        time = item,
                        timeHasPassed = freeAppointiments == true ? false : true
                    });
                }
            }
           
            List<Appointments> appointments = _appointmentsRepository.AppointmentsMade();

            foreach (var item in weekDays)
            {
                foreach (var agendamento in item.schedules)
                {

                    bool match = appointments.Any(a => a.dateTime.TimeOfDay == agendamento.time && a.dateTime == item.dayDateTime + agendamento.time);

                    if (match)
                        agendamento.barbersIds.AddRange(appointments.Where(a => a.dateTime.TimeOfDay == agendamento.time && usersBarber.Any(ub => ub.id == a.barber.id)).Select(a => a.barber.id).Distinct());
                }
            }
           


            List<ScheduleseBarbers> scheduleseBarbers = new List<ScheduleseBarbers>();

            foreach (var barber in usersBarber)
            {
                ScheduleseBarbers scheduleseBarber = new ScheduleseBarbers();
                scheduleseBarber.user = barber;
                foreach (var item in weekDays)
                {
                    item.schedules
                  .Where(i => i.barbersIds.Contains(barber.id))
                  .ToList()
                  .ForEach(i => i.free = false);
                }
                scheduleseBarber.weekDays = weekDays;
                scheduleseBarbers.Add(scheduleseBarber);
            }


            bool isBarber = SessionHelper.IsBarberUser();

            var jsonObject = new
            {
                schedulesBarbers = scheduleseBarbers,
                isBarber = isBarber,
            };

            return JsonSerializer.Serialize(jsonObject);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public List<WeekDays> ObterProximosSeteDias()
    {
        List<WeekDays> proximosDias = new List<WeekDays>();

        for (int i = 0; i < 7; i++)
        {
            WeekDays weekday = new WeekDays();
            DateTime dia = DateTime.Today.AddDays(i);
            string diaSemana = dia.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")); // Nome do dia da semana
            weekday.dayString = diaSemana;
            weekday.dayDateTime = dia;
            proximosDias.Add((weekday));
        }

        return proximosDias;
    }
    public List<TimeSpan> GerarHorarios(TimeSpan horaAbertura, TimeSpan horaFechamento)
    {
        var horarios = new List<TimeSpan>();
        var horaAtual = horaAbertura;

        while (horaAtual < horaFechamento)
        {
            horarios.Add(horaAtual);
            horaAtual = horaAtual.Add(TimeSpan.FromHours(1)); // Incrementa 1 hora
        }

        return horarios;
    }

    public string ObterDiaDaSemanaPorExtenso(int diaSemana)
    {
        return diaSemana switch
        {
            1 => "Segunda-feira",
            2 => "Terça-feira",
            3 => "Quarta-feira",
            4 => "Quinta-feira",
            5 => "Sexta-feira",
            6 => "Sábado",
            7 => "Domingo",
            _ => "Desconhecido"
        };
    }


    public bool BookingATime(AppointmentsDto appointmentsDTO)
    {
        try
        {  
            Cliente client = _clientRepository.SearchClientWichUserId(Convert.ToInt32(SessionHelper.UserId));

            foreach (var item in appointmentsDTO.dateTime)
            {
                if (_schedulingRepository.IsTimeBooked(client.id, appointmentsDTO.dayDateTime + item))
                {
                    return false;
                }
            }
                Appointments appointment = new Appointments();
                foreach (var date in appointmentsDTO.dateTime)
                {

                    appointment.client.id = client.id;

                    appointment.dateTime = appointmentsDTO.dayDateTime + date; 
                    appointment.customer.id = appointmentsDTO.customerId;
                    appointment.barber.id = appointmentsDTO.barberId;

                    _schedulingRepository.BookingATime(appointment);

                    _historicalSchedulesRepository.RegisterConfirmeOrCancelAppointments(appointment.idAppointments, appointment, StatusAppointmentEnum.Pendente);
                }
            
            return true;
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public string HasClientScheduling(int userId)
    {
        try
        {
            List<Appointments> appointiments = _appointmentsRepository.HasClientScheduling(userId);

            var jsonAppointiments = new
            {

                appointiments = appointiments,
                showNameBarber = appointiments.Any(a => a.showNameBarber == true) 
            };


            return JsonSerializer.Serialize(jsonAppointiments);
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

            Appointments oldAppointments = _appointmentsRepository.GetAppointmentsId(idAppointment);

            bool appointmentsCanCancel = _appointmentsRepository.CancelAppointment(idAppointment);

            _historicalSchedulesRepository.RegisterConfirmeOrCancelAppointments(idAppointment, oldAppointments, StatusAppointmentEnum.Cancelado);

            return appointmentsCanCancel;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    internal void ConfirmeAppointment(int idAppointment)
    {
        try
        {
           Appointments oldAppointments = _appointmentsRepository.GetAppointmentsId(idAppointment); 
            _appointmentsRepository.ConfirmeAppointments(idAppointment);
            _historicalSchedulesRepository.RegisterConfirmeOrCancelAppointments(idAppointment, oldAppointments, StatusAppointmentEnum.Confirmado);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
