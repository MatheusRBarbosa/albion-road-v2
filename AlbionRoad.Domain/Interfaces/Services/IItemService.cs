namespace AlbionRoad.Domain.Interfaces.Services;

public interface IItemService
{
    public IList<string> GetItemQueryParams(int batchSize);
}