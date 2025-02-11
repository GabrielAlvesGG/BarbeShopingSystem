namespace BarberShopSystem.Models
{
    public class AppointmentsDto
    {
        public TimeSpan dateTime { get; set; }
        public int customerId { get; set; }
        public int barberId { get; set; }
    }
}
