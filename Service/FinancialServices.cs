using BarberShopSystem.Models;

namespace BarberShopSystem.Service
{
    public class FinancialServices : IFinancialServices
    {
        public Task<bool> NewRegisterFinancial(CostsDto costsDto)
        {
			try
			{

			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
				throw;
			}
        }
    }
}
