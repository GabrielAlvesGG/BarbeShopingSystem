namespace BarberShopSystem.Models
{
    public class RecoveryPassword
    {
        public int id { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public bool used { get; set; } = false;
        public int idUser { get; set; }
    }
}
