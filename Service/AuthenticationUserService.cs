using BarberShopSystem.Enums;
using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BarberShopSystem.Service;

public class AuthenticationUserService : IAuthenticationUserService
{
    private readonly UserService _userService;
    public AuthenticationUserService(UserService userService)
    {
        _userService = userService;
    }  
    public bool AuthenticateGoogleUser(AuthenticateResult authenticate)
    {
        if (!authenticate.Succeeded)
            return false;

        var claims = authenticate.Principal.Identities
                          .FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });

        User user = new User();
        user.email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        User userFound = _userService.GetUser(user);

        if (userFound == null)
        {
            user.nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            user.tipoUsuario = TipoUsuarioEnum.Cliente;
            _userService.InsertOrUpdateUserAsync(user,null);
        }
        else
        {
            SessionHelper.StartSessionLogger(userFound); 
        }


        return true;
    }
}
