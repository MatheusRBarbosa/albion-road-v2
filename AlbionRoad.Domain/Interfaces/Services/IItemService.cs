using AlbionRoad.Domain.Models;
namespace AlbionRoad.Domain.Interfaces.Services;

public interface IItemService
{
    public IList<string> GetItemQueryParams(int batchSize);
    public IList<Profit> GetItemsProfit(IList<Price> prices);
}