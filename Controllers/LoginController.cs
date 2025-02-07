using BarberShopSystem.Models;
using BarberShopSystem.Helpers;
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
    private readonly UserService _userService;
    private readonly LoginService _loginService;
    private readonly AuthenticationUserService _authenticationUserService;

    public LoginController(RecoveryPasswordService recoveryPasswordService,
                           UserService userService, 
                           LoginService loginService, 
                           AuthenticationUserService authenticationUserService)
    {
        _recoveryPasswordService = recoveryPasswordService;
        _userService = userService;
        _loginService = loginService;
        _authenticationUserService = authenticationUserService;
    }

    public IActionResult Login()
    {
        return SessionHelper.IsUserLoggedIn()
            ? RedirectToAction("Index", "Home")
            : View();
    }

    public IActionResult RecoveryPassword()
    {
        return (SessionHelper.IsUserLoggedIn() ?
            RedirectToAction("Index", "Home") :
            View());
    }

    public bool LoginUser([FromBody] loginDto login)
    {
        try
        {
            SessionHelper.StartSessionLogger(_loginService.LoginValidate(login));
            return SessionHelper.IsUserLoggedIn();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void Logout()
    {
        SessionHelper.ClearSession();
    }

    public bool SendRecoveryCode([FromBody] string login)
    {
        try
        {
            Client user = _recoveryPasswordService.LoginConfirm(login);
            return user != null ? _recoveryPasswordService.SendCodConfirm(user) : false;
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

    public void ChangePassword([FromBody] ResetPasswordDto resetPasswordDto)
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

        bool userAuthenticated = _authenticationUserService.AuthenticateGoogleUser(authenticateResult);

        return userAuthenticated
            ? RedirectToAction("Index", "Home")
            : RedirectToAction("Login", "Login");
    }

}
