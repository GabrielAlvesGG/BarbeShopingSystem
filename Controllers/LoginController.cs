using BarberShopSyste.Models;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BarberShopSystem.ModelsRepository;
using BarberShopSystem.Enums;

namespace BarberShopSystem.Controllers;

public class LoginController : Controller
{
    private readonly RecoveryPasswordService _recoveryPasswordService;
    private readonly UserService _UserService;

    public LoginController(RecoveryPasswordService recoveryPasswordService, UserService userService)
    {
        _recoveryPasswordService = recoveryPasswordService;
        _UserService = userService;
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
        Client clientLoggedIn = loginService.LoginValidate(login);
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
            Client user = _recoveryPasswordService.LoginConfirm(login);
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
    public IActionResult LoginWithGoogle()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "Login", null, Request.Scheme)
        };
        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return RedirectToAction("Login");

        var claims = authenticateResult.Principal.Identities
                          .FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });

        Client user = new Client();
        user.email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        Client userFound = _UserService.GetUser(user); 

        if (userFound == null)
        {
            user.nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            user.tipoUsuario = TipoUsuarioEnum.Cliente;
            _UserService.InsertOrUpdateUser(user);
        }
        else
        {
            user = userFound;
            SessionHelper.UserId = user.id;
            SessionHelper.UserName = user.nome == null ? string.Empty : user.nome;
            SessionHelper.UserType = user.tipoUsuario.ToString();
        }


        return RedirectToAction("Login", "Login");
    }
}
