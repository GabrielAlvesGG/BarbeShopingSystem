using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;   

    public CustomerService(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public List<Customer> GetCustomers()
    {
        try
        {
            return _customerRepository.GetAllCustomers();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
