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
    public IActionResult ListServices()
    {
        return View();
    }
    [HttpPost]
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
    [HttpGet]
    public IActionResult ListAllServices()
    {
        try
        {
            return Json(_servicesProvidedService.GetAllServices());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    [HttpPost]
    public IActionResult EditServices([FromBody]int idServices)
    {
        try
        {
            return Json(_servicesProvidedService.GetServicesId(idServices));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    [HttpPost]
    public IActionResult DeleteServices([FromBody] int idServices)
    {
        try
        {
            _servicesProvidedService.DeleteServieces(idServices);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
