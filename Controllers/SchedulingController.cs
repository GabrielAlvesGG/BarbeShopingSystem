using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;

namespace BarberShopSystem.Controllers;

public class SchedulingController : Controller
{

    private readonly SchedulingService _schedulingService;
    private readonly CustomerService _customerService;

    public SchedulingController(SchedulingService shedulingService,CustomerService customerService)
    {
        _schedulingService = shedulingService;
        _customerService = customerService;
    }
    public IActionResult Scheduling()
    {
        return View();
    } 
    public IActionResult MyScheduling()
    {
        return View();
    }

    public List<Schedules> GetScheduling() // Por enquanto ele só está pegando o horário e dividindo durante o dia todo.
    {        
        return _schedulingService.GetScheduling(); ;
    }
    public void BookingATime([FromBody] AppointmentsDto appointments)
    {
      _schedulingService.BookingATime(appointments);
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

    public List<Customer> GetCustomers()
    {
        try
        {
            return _customerService.GetCustomers();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
