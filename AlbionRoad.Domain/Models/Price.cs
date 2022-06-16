using System.Text.Json.Serialization;
namespace AlbionRoad.Domain.Models;

public class Price
{
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    [JsonPropertyName("city")]
    public string City { get; set; } = null!;

    [JsonPropertyName("quality")]
    public int Quality { get; set; }

    [JsonPropertyName("sell_price_min")]
    public int SellPriceMin { get; set; }

    [JsonPropertyName("sell_price_min_date")]
    public DateTime SellPriceMinDate { get; set; }

    [JsonPropertyName("sell_price_max")]
    public int SellPriceMax { get; set; }

    [JsonPropertyName("sell_price_max_date")]
    public DateTime SellPriceMaxDate { get; set; }

    [JsonPropertyName("buy_price_min")]
    public int BuyPriceMin { get; set; }

    [JsonPropertyName("buy_price_min_date")]
    public DateTime BuyPriceMinDate { get; set; }

    [JsonPropertyName("buy_price_max")]
    public int BuyPriceMax { get; set; }

    [JsonPropertyName("buy_price_max_date")]
    public DateTime BuyPriceMaxDate { get; set; }
}