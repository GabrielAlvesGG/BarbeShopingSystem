namespace BarberShopSystem
{
    public class Barber
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public string? Email { get; set; }
        public String? PassWord { get; set; }
        public string? cpf { get; set; }
    }
}
