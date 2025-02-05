using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;

namespace BarberShopSystem.Controllers;

public class SchedulingController : Controller
{

    private readonly SchedulingService _schedulingService;

    public SchedulingController(SchedulingService shedulingService)
    {
        _schedulingService = shedulingService;
    }
    public IActionResult Scheduling()
    {
        return View();
    }

    public List<Schedules> GetScheduling() // Por enquanto ele só está pegando o horário e dividindo durante o dia todo.
    {        
        return _schedulingService.GetScheduling(); ;
    }
    public void BookingATime([FromBody] string time)
    {
      _schedulingService.BookingATime(time);
    }

    public List<Appointments> HasSchelulingClient()
    {
        try
        {
            return _schedulingService.HasClientScheduling(Convert.ToInt32(SessionHelper.UserId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public bool CancelAppointment([FromBody] int idAppointment)
    {
        try
        {
           return _schedulingService.CancelAppointment(idAppointment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
