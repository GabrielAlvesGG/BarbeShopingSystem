namespace BarberShopSystem.Models;

public class ScheduleseBarbers
{   
    public User user { get; set; } = new User();
    public List<WeekDays> weekDays { get; set; } = new List<WeekDays>();
}
