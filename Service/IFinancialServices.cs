using BarberShopSystem.Models;

namespace BarberShopSystem.Service
{
    public interface IFinancialServices
    {
        public Task<bool> NewRegisterFinancial(CostsDto costsDto);
    }
}
