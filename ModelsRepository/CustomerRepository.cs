using BarberShopSystem.Data;
using BarberShopSystem.Models;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.ModelsRepository;

public class CustomerRepository : DataBaseRepository
{
    public CustomerRepository(IConfiguration configuration) : base(configuration) { }

    public List<Customer> GetAllCustomers()
    {
		try
		{ List<Customer> customers = new List<Customer>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM servicos", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        customers.Add(new Customer
                        {
                            id = reader.GetInt32("Id"),
                            name = reader.GetString("Nome"),
                            description = reader.GetString("Descricao"),
                            price = reader.GetDouble("Preco"),
                        }); 
                    }
                }
            }
            return customers;
        }
		catch (Exception ex)
		{
            Console.WriteLine(ex.Message);
			throw;
		}
    }
}
