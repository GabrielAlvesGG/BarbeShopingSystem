using MySql.Data.MySqlClient;

namespace BarberShopSystem
{
    public class ClientService
    {
        public List<Client> ListAllClient()
        {
            try
            {
                ClientRepository clientRepository = new ClientRepository();
                
                return clientRepository.ListAllClient(); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
        public void InsertClient(Client client)
        {
            try
            {
                new ClientRepository().InsertClient(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public Client GetClient(int idOldClient)
        {
            try
            {  
                return new ClientRepository().GetClient(idOldClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void DeleteClient(int idClient)
        {
            try
            {
               new ClientRepository().DeleteClient(idClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }

}
