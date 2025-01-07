namespace BarberShopSystem.Models
{
    public class ClientModel
    {
        public void teste()
        {
			try
			{
				Client client = new Client();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
        }
    }
}
