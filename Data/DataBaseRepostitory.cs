using MySql.Data.MySqlClient;

namespace BarberShopSystem.Data
{
    public class DataBaseRepostitory
    {
        public MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection("server=localhost;database=barbeshopsystem;uid=root;pwd=masterkey;");
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public void GetAll()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
