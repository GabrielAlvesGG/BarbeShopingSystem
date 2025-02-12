using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopSystem.Controllers;

public class ServicesProvidedController : Controller
{
    public readonly ServicesProvidedService _servicesProvidedService;
    public ServicesProvidedController(ServicesProvidedService servicesProvidedServce)
    {
        _servicesProvidedService = servicesProvidedServce;
    }
    public IActionResult RegisterServicesProvided()
    {
        return View();
    }
    public IActionResult RegisterServer([FromBody]ServicesProvided serviceProvided)
    {
        try
        {
            _servicesProvidedService.InsertOrUpdateServicesProvided(serviceProvided);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
