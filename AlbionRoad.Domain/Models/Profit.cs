namespace AlbionRoad.Domain.Models;

public class Profit
{
    public int Value { get; set; }
    public string ItemId { get; set; } = null!;
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
}