using AlbionRoad.Domain.Models;
namespace AlbionRoad.Domain.Interfaces.Services;

public interface ICacheService
{
    public Task<IList<Price>?> GetPricesAsync();
    public Task<IList<Profit>?> GetProfitsAsync(Route route);
    public void SetRawPrices(IList<Price> items);
    public void SetItemsProfit(Route route, IList<Profit> profits);
}