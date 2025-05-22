using BarberShopSystem.Helpers;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using System.Configuration;

namespace BarberShopSystem.Service;

public interface ILoginService
{

    public User LoginValidate(loginDto login);
}
