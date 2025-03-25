using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;

namespace BarberShopSystem.Controllers;

[Route("Scheduling")] // Define a base da rota para evitar conflitos
public class SchedulingController : Controller
{
    private readonly SchedulingService _schedulingService;
    private readonly CustomerService _customerService;

    public SchedulingController(SchedulingService shedulingService, CustomerService customerService)
    {
        _schedulingService = shedulingService;
        _customerService = customerService;
    }
    [HttpGet("Scheduling")]
    public IActionResult Scheduling() => View();

    [HttpGet("MyScheduling")]
    public IActionResult MyScheduling() => View();

    [HttpGet("GetScheduling")]
    public IActionResult GetScheduling()
    {
        return Json(_schedulingService.GetScheduling());
    }

    [HttpPost("BookingATime")]
    public IActionResult BookingATime([FromBody] AppointmentsDto appointments)
    {
        
        return Json(_schedulingService.BookingATime(appointments));
    }

    [HttpGet("HasSchelulingClient")]
    public IActionResult HasSchelulingClient()
    {
        try
        {
            return Json(_schedulingService.HasClientScheduling(Convert.ToInt32(SessionHelper.UserId)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    [HttpPost("CancelAppointment")]
    public IActionResult CancelAppointment([FromBody] int idAppointment)
    {
        try
        {
            return Json(_schedulingService.CancelAppointment(idAppointment));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    [HttpGet("GetCustomers")]
    public IActionResult GetCustomers()
    {
        try
        {
            return Json(_customerService.GetCustomers());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    [HttpPost("ConfirmeAppointment")]
    public IActionResult ConfirmeAppointment([FromBody]int idAppointment)
    {
        try
        {
            _schedulingService.ConfirmeAppointment(idAppointment);
            return Ok();
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    } 
}

