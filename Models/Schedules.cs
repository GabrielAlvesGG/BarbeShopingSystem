﻿namespace BarberShopSystem.Models;

public class Schedules
{
    public bool free { get; set; }
    public TimeSpan time{ get; set; }
    public List<int> barbersIds { get; set; } = new List<int>();
    public bool timeHasPassed { get; set; }
}
