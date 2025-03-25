namespace BarberShopSystem.Models
{
    public class AppointmentsDto // Implementar uma nova variável que ira identificar o dia que será agendado o serviço
    {
        public List<TimeSpan> dateTime { get; set; }
        public int customerId { get; set; }
        public int barberId { get; set; }
        public bool isBarberLogged { get; set; }
        public DateTime dayDateTime { get; set; }
    }
}
