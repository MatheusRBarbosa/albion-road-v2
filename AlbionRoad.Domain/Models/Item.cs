namespace AlbionRoad.Domain.Models;
using System.Text.Json.Serialization;
public class Item
{
    public string Index { get; set; } = null!;
    public string UniqueName { get; set; } = null!;
    public string LocalizationNameVariable { get; set; } = null!;

    public Boolean Eligible()
    {
        return EligibleTier();
    }

    /// <summary>
    /// Define if the item is eligible to concat to request in albion-data api
    /// </summary>
    private Boolean EligibleTier()
    {
        return UniqueName[0] == 'T' && (
            UniqueName[1] == '2' ||
            UniqueName[1] == '3' ||
            UniqueName[1] == '4' ||
            UniqueName[1] == '5' ||
            UniqueName[1] == '6'
        );
    }
}

public class LocalizationName
{
    [JsonPropertyName("EN-US")]
    public string EnUS { get; set; } = null!;

    [JsonPropertyName("PT-BR")]
    public string PtBr { get; set; } = null!;
}
