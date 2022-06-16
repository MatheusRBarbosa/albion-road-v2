using System.Text.Json;
using AlbionRoad.Domain.Models;
using AlbionRoad.Domain.Interfaces.Services;

namespace AlbionRoad.Domain.Services;

public class ItemService : IItemService
{
    public string GetItemQueryParams()
    {
        var itemsIds = GetItemsIds();
        return String.Join(",", itemsIds.ToArray());
    }

    public IList<string> GetItemQueryParams(bool baches)
    {
        throw new NotImplementedException();
    }

    private IList<string> GetItemsIds()
    {
        var items = GetItemsFromFile();
        var itemIds = items
            .Where(i => i.UniqueName[0] == 'T')
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