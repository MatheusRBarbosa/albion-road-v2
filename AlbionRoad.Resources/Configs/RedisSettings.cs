namespace AlbionRoad.Resources.Configs;

public class RedisSettings
{
    public const string SECTION = "Redis";
    public string? Host { get; set; }
    public int Port { get; set; }
    public int TTL { get; set; }
}