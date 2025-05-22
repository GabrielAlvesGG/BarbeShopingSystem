
using Microsoft.AspNetCore.Authentication;


namespace BarberShopSystem.Service;

public interface IAuthenticationUserService 
{
    public bool AuthenticateGoogleUser(AuthenticateResult authenticate);

}
