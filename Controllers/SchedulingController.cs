using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using static BarberShopSystem.ModelsRepository.AppointmentsRepository;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarberShopSystem.Controllers;

[Route("Scheduling")]
public class SchedulingController : Controller
{
    private readonly SchedulingService _schedulingService;
    private readonly ICustomerService _customerService;
    private readonly ILogger<SchedulingController> _logger;
    private readonly IWebHostEnvironment _env;

    public SchedulingController(SchedulingService shedulingService, ICustomerService customerService, ILogger<SchedulingController> logger, IWebHostEnvironment env)
    {
        _schedulingService = shedulingService;
        _customerService = customerService;
        _logger = logger;
        _env = env;
    }

    [HttpGet("Scheduling")]
    public IActionResult Scheduling() => View();

    [HttpGet("MyScheduling")]
    public IActionResult MyScheduling() => View();

    [HttpGet("GetScheduling")]
    public IActionResult GetScheduling()
    {
        try
        {
            _logger.LogInformation("GetScheduling called");
            var result = _schedulingService.GetScheduling();
            _logger.LogDebug("GetScheduling result type: {Type}", result?.GetType()?.FullName);

            if (result is string s)
            {
                _logger.LogDebug("GetScheduling returning raw string JSON (len={Len})", s?.Length ?? 0);
                return Content(s, "application/json; charset=utf-8");
            }

            var json = JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            );
            _logger.LogDebug("GetScheduling returning serialized JSON (len={Len})", json?.Length ?? 0);
            return Content(json, "application/json; charset=utf-8");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetScheduling failed");
            return StatusCode(500, new
            {
                message = "Erro ao obter agendamentos.",
                detail = _env.IsDevelopment() ? ex.Message : null
            });
        }
    }

    [HttpPost("BookingATime")]
    public IActionResult BookingATime([FromBody] AppointmentsDto appointments)
    {
        try
        {
            _logger.LogInformation("BookingATime called {@Appointments}", appointments);
            var ok = _schedulingService.BookingATime(appointments);
            return Json(ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BookingATime failed {@Appointments}", appointments);
            return StatusCode(500, new { message = "Erro ao agendar horário.", detail = _env.IsDevelopment() ? ex.Message : null });
        }
    }

    [HttpGet("HasSchelulingClient")]
    public IActionResult HasSchelulingClient()
    {
        try
        {
            _logger.LogInformation("HasSchelulingClient called for user {UserId}", SessionHelper.UserId);
            return Json(_schedulingService.HasClientScheduling(Convert.ToInt32(SessionHelper.UserId)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HasSchelulingClient failed");
            return StatusCode(500, new { message = "Erro ao verificar agendamentos do cliente.", detail = _env.IsDevelopment() ? ex.Message : null });
        }
    }

    [HttpPost("CancelAppointment")]
    public IActionResult CancelAppointment([FromBody] int idAppointment)
    {
        try
        {
            _logger.LogInformation("CancelAppointment called {IdAppointment}", idAppointment);
            return Json(_schedulingService.CancelAppointment(idAppointment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CancelAppointment failed {IdAppointment}", idAppointment);
            return StatusCode(500, new { message = "Erro ao cancelar agendamento.", detail = _env.IsDevelopment() ? ex.Message : null });
        }
    }

    [HttpGet("GetCustomers")]
    public IActionResult GetCustomers()
    {
        try
        {
            _logger.LogInformation("GetCustomers called");
            return Json(_customerService.GetCustomers());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetCustomers failed");
            return StatusCode(500, new { message = "Erro ao obter serviços.", detail = _env.IsDevelopment() ? ex.Message : null });
        }
    }

    [HttpPost("ConfirmeAppointment")]
    public IActionResult ConfirmeAppointment([FromBody] int idAppointment)
    {
        try
        {
            _logger.LogInformation("ConfirmeAppointment called {IdAppointment}", idAppointment);
            _schedulingService.ConfirmeAppointment(idAppointment);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmeAppointment failed {IdAppointment}", idAppointment);
            return StatusCode(500, new { message = "Erro ao confirmar agendamento.", detail = _env.IsDevelopment() ? ex.Message : null });
        }
    }
}

