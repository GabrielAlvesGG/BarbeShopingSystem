using BarberShopSystem.Models;
using BarberShopSystem.Helpers;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BarberShopSystem.ModelsRepository;
using BarberShopSystem.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BarberShopSystem.Controllers;

public class LoginController : Controller
{
    private readonly IRecoveryPasswordService _recoveryPasswordService;
    private readonly UserService _userService;
    private readonly ILoginService _loginService;
    private readonly IAuthenticationUserService _authenticationUserService;

    public LoginController(IRecoveryPasswordService recoveryPasswordService,
                           UserService userService,
                           ILoginService loginService,
                           IAuthenticationUserService authenticationUserService)
    {
        _recoveryPasswordService = recoveryPasswordService;
        _userService = userService;
        _loginService = loginService;
        _authenticationUserService = authenticationUserService;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return SessionHelper.IsUserLoggedIn()
            ? RedirectToAction("Index", "Home")
            : View();
    }

    [AllowAnonymous]
    public IActionResult RecoveryPassword()
    {
        return (SessionHelper.IsUserLoggedIn() ?
            RedirectToAction("Index", "Home") :
            View());
    }

    [AllowAnonymous]
    public async Task<bool> LoginUser([FromBody] loginDto login)
    {
        try
        {
            // 1) Validação com SQL do jeito que você já faz
            var user = _loginService.LoginValidate(login);

            // Se falhou autenticação, retorna false
            if (user == null || user.id <= 0)
                return false;

            // 2) Mantém sua sessão (sua lógica)
            SessionHelper.StartSessionLogger(user);

            // 3) Cria o cookie de autenticação (LEMBRAR DE MIM)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.nome ?? login.login),
                new Claim(ClaimTypes.Email, login.login ?? string.Empty),
                new Claim(ClaimTypes.Role, user.tipoUsuario.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = login.rememberMe,                 // <- “Lembrar de mim”
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) // casa com Program.cs
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                props
            );

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        SessionHelper.ClearSession();
    }

    [AllowAnonymous]
    public bool SendRecoveryCode([FromBody] string login)
    {
        try
        {
            User user = _recoveryPasswordService.LoginConfirm(login);
            return user != null ? _recoveryPasswordService.SendCodConfirm(user) : false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    public IActionResult LoginWithGoogle()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "Login", null, Request.Scheme)
        };
        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        // Se usar Google de fato, o certo é autenticar com o esquema do Google:
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        bool userAuthenticated = _authenticationUserService.AuthenticateGoogleUser(authenticateResult);

        return userAuthenticated
            ? RedirectToAction("Index", "Home")
            : RedirectToAction("Login", "Login");
    }
}
