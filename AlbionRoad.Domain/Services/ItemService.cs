using System.Text.Json;
using AlbionRoad.Domain.Models;
using AlbionRoad.Domain.Interfaces.Services;

namespace AlbionRoad.Domain.Services;

public class ItemService : IItemService
{
    public IList<string> GetItemQueryParams(int batchSize)
    {
        var itemsIds = GetItemsIds();
        IList<string> batches = new List<string>();

        var batch = new List<string>();
        foreach (var item in itemsIds)
        {
            if (batch.Count < batchSize)
            {
                batch.Add(item);
            }
            else
            {
                batches.Add(string.Join(",", batch));
                batch = new List<string>();
            }
        }

        return batches;
    }

    public IList<Profit> GetItemsProfit(IList<Price> prices, Route route)
    {

        (var pricesFrom, var pricesTo) = SanitizePrices(prices, route);

        var profits = new List<Profit>();

        Parallel.For(0, pricesFrom.Count, i =>
        {
            var priceFrom = pricesFrom[i];
            var priceTo = pricesTo.First(to => to.ItemId == priceFrom.ItemId);
            var profitValue = priceFrom.SellPriceMin - priceTo.SellPriceMin;
            var profit = new Profit
            {
                ProfitValue = profitValue,
                BuyValue = priceFrom.SellPriceMin,
                SellValue = priceTo.SellPriceMin,
                ItemId = priceFrom.ItemId,
                From = route.From.Name,
                To = route.To.Name
            };

            profits.Add(profit);

        });

        profits = profits
                .Where(p => p.BuyValue != 0 && p.SellValue != 0)
                .OrderByDescending(p => p.ProfitValue)
                .Take(15)
                .ToList();

        return profits;
    }

    private IList<string> GetItemsIds()
    {
        var items = GetItemsFromFile();
        var itemIds = items
            .Where(i => i.Eligible())
            .Select(i => i.UniqueName)
            .ToList();

        return itemIds;
    }

    private IList<Item> GetItemsFromFile()
    {
        StreamReader r = new StreamReader("../AlbionRoad.Resources/items.json");
        string toJson = r.ReadToEnd();
        IList<Item> items = JsonSerializer.Deserialize<List<Item>>(toJson)!;

        return items;
    }

    private (IList<Price>, IList<Price>) SanitizePrices(IList<Price> prices, Route route)
    {
        var sanitizedPricesFrom = new List<Price>();
        var sanitizedPricesTo = new List<Price>();

        foreach (var price in prices)
        {
            var itemCity = price.City.Replace(" ", String.Empty);
            if (itemCity == route.From.Name)
            {
                sanitizedPricesFrom.Add(price);
            }
            else if (itemCity == route.To.Name)
            {
                sanitizedPricesTo.Add(price);
            }
        }

        return (sanitizedPricesFrom, sanitizedPricesTo);
    }
}