namespace BarberShopSystem.Models;

public class OpeningHours
{
    public int id { get; set; }
    public int barbeariaId { get; set; }
    public int diaSemana { get; set; }
    public TimeSpan horaAbertura { get; set; }
    public TimeSpan horaFechamento { get; set; }

}
