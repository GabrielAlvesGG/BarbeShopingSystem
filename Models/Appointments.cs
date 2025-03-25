using BarberShopSystem.Enums;

namespace BarberShopSystem.Models
{
    public class Appointments
    {
        
        public int idAppointments { get; set; }
        public Cliente client { get; set; } = new Cliente();
        public Barber barber{ get; set; } = new Barber();
        public TimeSpan appointments { get; set; }
        public Customer customer { get; set; } = new Customer();
        public DateTime dateTime { get; set; }
        public string statusAppointment { get; set; }
        public string? nameShowBarberOrCliente { get; set; }
        public bool showNameBarber { get; set; }
    }
}
