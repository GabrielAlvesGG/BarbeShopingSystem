using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace BarberShopSystem.Data
{
    public class DataBaseRepository
    {
        private readonly IConfiguration _configuration;

        public DataBaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected MySqlConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        }
    }
}