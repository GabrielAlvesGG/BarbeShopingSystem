﻿using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Google.Protobuf.WellKnownTypes;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;

namespace BarberShopSystem.Service;

public class SchedulingService
{
	private readonly SchedulingRepository _schedulingRepository;
	private readonly AppointmentsRepository _appointmentsRepository;

	public SchedulingService(SchedulingRepository schedulingRepository, AppointmentsRepository appointmentsRepository)
    {
        _schedulingRepository = schedulingRepository;
        _appointmentsRepository = appointmentsRepository;
    }

    public List<Schedules> GetScheduling()
    {
		try
		{

            OpeningHours openingHours =  _schedulingRepository.GetScheduling();
            List<Schedules> schedules = new List<Schedules>();
            foreach (var item in GerarHorarios(openingHours.horaAbertura, openingHours.horaFechamento))
            {
                schedules.Add(new Schedules()
                {
                    free = true,
                    time = item
                });
            }
            List<DateTime> appointments = _appointmentsRepository.AppointmentsMade();

            foreach (var agendamento in schedules)
            {

                bool match = appointments.Any(a => a.TimeOfDay == agendamento.time);

                if (match)
                    agendamento.free = false;
                
            }

            return schedules;

        }
		catch (Exception ex)
		{

			throw;
		}
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

    public void BookingATime(string time)
    {
        _schedulingRepository.BookingATime(time);
    }

    public List<Appointments> HasClientScheduling(int userId)
    {
        try
        {
            return _appointmentsRepository.HasClientScheduling(userId);
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
            return _appointmentsRepository.CancelAppointment(idAppointment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
