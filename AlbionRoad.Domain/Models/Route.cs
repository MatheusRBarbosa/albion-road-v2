namespace AlbionRoad.Domain.Models;

public class Route
{
    public City From { get; set; } = null!;
    public City To { get; set; } = null!;

    public string ToUrlQueryParam
    {
        get => $"locations={From.Name},{To.Name}";
    }
}