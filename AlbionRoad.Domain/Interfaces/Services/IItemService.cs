namespace AlbionRoad.Domain.Interfaces.Services;

public interface IItemService
{
    public string GetItemQueryParams();

    public IList<string> GetItemQueryParams(bool baches);
}