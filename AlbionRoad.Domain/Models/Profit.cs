namespace AlbionRoad.Domain.Models;

public class Profit
{
    public int ProfitValue { get; set; }
    public int BuyValue { get; set; }
    public int SellValue { get; set; }
    public int ItemQuality { get; set; }
    public string ItemId { get; set; } = null!;
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
}