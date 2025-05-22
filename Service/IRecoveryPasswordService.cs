using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;

namespace BarberShopSystem.Service;

public interface IRecoveryPasswordService
{
 
    public User LoginConfirm(string login);

    public bool SendCodConfirm(User user);    

    public bool ValidateToken(string token);

    public void ResetPasswored(ResetPasswordDto resetPasswordDto);
    
}
