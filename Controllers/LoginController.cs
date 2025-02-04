using BarberShopSyste.Models;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace BarberShopSystem.Controllers;

public class LoginController : Controller
{
    private readonly RecoveryPasswordService _recoveryPasswordService;

    public LoginController(RecoveryPasswordService recoveryPasswordService)
    {
        _recoveryPasswordService = recoveryPasswordService;
    }
    public IActionResult Login()
    {
        if (SessionHelper.IsUserLoggedIn())
            return RedirectToAction("Index", "Home");
        else
            return View();
    }

    public IActionResult RecoveryPassword()
    {
        if (SessionHelper.IsUserLoggedIn())
            return RedirectToAction("Index", "Home");
        else
            return View();
    }
    public IActionResult teste()
    {
            return View();
    }

    public bool Logar([FromBody] loginDto login)
    {
        LoginService loginService = new LoginService();
        Usuario clientLoggedIn = loginService.LoginValidate(login);
        SessionHelper.UserId = clientLoggedIn.id;
        SessionHelper.UserName = clientLoggedIn.nome == null ? string.Empty: clientLoggedIn.nome;
        SessionHelper.UserType = clientLoggedIn.tipoUsuario.ToString();

        return SessionHelper.IsUserLoggedIn(); 
    }

    public void Logout()
    {
        SessionHelper.ClearSession();
    }
    public bool IsUserLoggedIn()
    {
        return SessionHelper.IsUserLoggedIn();
    }
    public bool IsUserMaster()
    {
        try
        {
            return SessionHelper.IsMasterUser();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public bool CallRecoveryPassword([FromBody] string login)
    {
        try
        {
            Usuario user = _recoveryPasswordService.LoginConfirm(login);
            if (user != null)
                return _recoveryPasswordService.SendCodConfirm(user);
            else
                return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public bool ValidateToken([FromBody] string token)
    {
        try
        {
            return _recoveryPasswordService.ValidateToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            _recoveryPasswordService.ResetPasswored(resetPasswordDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
