namespace AlbionRoad.Domain.Models;
public class Item
{
    public string LocalizationNameVariable { get; set; } = null!;
    public string LocalizationDescriptionVariable { get; set; } = null!;
    public string Index { get; set; } = null!;
    public string UniqueName { get; set; } = null!;

    public Boolean Eligible()
    {

        return EligibleTier();
    }

    private Boolean EligibleTier()
    {
        return UniqueName[0] == 'T' && (
            UniqueName[1] == '2' ||
            UniqueName[1] == '3' ||
            UniqueName[1] == '4' ||
            UniqueName[1] == '5' ||
            UniqueName[1] == '6' ||
            UniqueName[1] == '7' ||
            UniqueName[1] == '8'
        );
    }
}
