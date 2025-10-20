using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BarbeShopingSystem.Controllers;

    public class FinancialController : Controller
    {
        public IActionResult FinancialRegister()
        {
            return View();
        }
    }
