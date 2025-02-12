using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;

namespace BarberShopSystem.Service
{
    public class ServicesProvidedService
    {
        public readonly ServicesProvidedRepository _servicesProvidedRepository;
        public ServicesProvidedService(ServicesProvidedRepository servicesProvidedRepository)
        {
            _servicesProvidedRepository = servicesProvidedRepository;
        }

        internal void InsertOrUpdateServicesProvided(ServicesProvided serviceProvided)
        {
			try
			{
                _servicesProvidedRepository.InsertOrUpdateProvided(serviceProvided);
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
				throw;
			}
        }
    }
}

