namespace BarberShopSystem.Models;

public class WeekDays
{
    public DateTime dayDateTime { get; set; }
    public string dayString { get; set; }

    public List<Schedules> schedules { get; set; } = new List<Schedules>();
}

public class Schedules
{
    public bool free { get; set; }
    public TimeSpan time{ get; set; }
    public List<int> barbersIds { get; set; } = new List<int>();
    public bool timeHasPassed { get; set; }
}
