using BarberShopSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BarbeShopingSystem.Controllers;

public class FinancialController : Controller
{
    public IActionResult FinancialRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NewRegisterFinancial([FromBody] CostsDto costsDto)
    {
        if (costsDto == null)
        {
            return BadRequest("Dados inválidos");
        }

        try
        {
            // Aqui você pode adicionar a lógica para salvar os dados no banco de dados
            return Ok(new { message = "Lançamento salvo com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno", error = ex.Message });
        }
    }
}
