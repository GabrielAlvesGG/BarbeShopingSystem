using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public interface ICustomerService
{
    public List<Customer> GetCustomers();
}
