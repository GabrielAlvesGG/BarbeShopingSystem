using BarberShopSystem.Data;
using BarberShopSystem.Models;
using BarberShopSystem.ModelsRepository;
using MySql.Data.MySqlClient;

namespace BarberShopSystem.Service
{
    public class ServicesProvidedService
    {
        public readonly ServicesProvidedRepository _servicesProvidedRepository;
        public ServicesProvidedService(ServicesProvidedRepository servicesProvidedRepository)
        {
            _servicesProvidedRepository = servicesProvidedRepository;
        }

        internal List<ServicesProvided> GetAllServices()
        {
            try
            {
               return _servicesProvidedRepository.AllServices();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal ServicesProvided GetServicesId(int idServices)
        {
            try
            {
                return _servicesProvidedRepository.GetServicesId(idServices);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal void InsertOrUpdateServicesProvided(ServicesProvided serviceProvided)
        {
			try
			{
                InsertOrUpdateProvided(serviceProvided);
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
				throw;
			}
        }
        internal void InsertOrUpdateProvided(ServicesProvided serviceProvided)
        {
            try
            {

                if (serviceProvided.id == 0)
                {
                    _servicesProvidedRepository.InsertServicesProvided(serviceProvided);
                }
                else
                    _servicesProvidedRepository.UpdateServicesProvided(serviceProvided);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal void DeleteServieces(int idServices)
        {
            try
            {
                _servicesProvidedRepository.DeleteServices(idServices);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

