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

    public IList<Profit> GetItemsProfit(IList<Price> prices)
    {
        var profits = new List<Profit>();

        foreach (var price in prices)
        {
            // Colocar na lista enquanto o tamanho for menos 10
            // Mas se o tamanho for 10, trocar apenas de o profit.value for maior
            // Fazer isso apenas se as cidades tiverem relacao com from e to
        }

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
}