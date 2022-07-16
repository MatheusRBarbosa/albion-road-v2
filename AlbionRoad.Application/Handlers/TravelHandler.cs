using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

using AlbionRoad.Domain.Models;
using AlbionRoad.Domain.Enums;
using AlbionRoad.Domain.Exceptions;
using AlbionRoad.Domain.Interfaces.Services;
using AlbionRoad.Resources.Configs;

namespace AlbionRoad.Application.Handlers;

public class TravelHandler
{
    private readonly IMapper mapper;
    private readonly AlbionDataSettings albionData;
    private readonly IHttpClientFactory httpFactory;
    private readonly IItemService itemService;
    private readonly ICacheService cacheService;

    public TravelHandler(
        IMapper mapper,
        IOptions<AlbionDataSettings> albionData,
        IHttpClientFactory httpFactory,
        IItemService itemService,
        ICacheService cacheService
    )
    {
        this.mapper = mapper;
        this.albionData = albionData.Value;
        this.httpFactory = httpFactory;
        this.itemService = itemService;
        this.cacheService = cacheService;
    }

    public async Task<IList<Profit>> Travel(int from, int to)
    {
        var route = GetRoute(from, to);

        IList<Price>? prices = new List<Price>();

        var profits = await cacheService.GetProfitsAsync(route);
        if (profits != null)
        {
            return profits;
        }

        prices = await cacheService.GetPricesAsync();
        if (prices != null)
        {
            profits = itemService.GetItemsProfit(prices, route);
            cacheService.SetItemsProfit(route, profits);
            return profits;
        }

        prices = await GetPricesFromHttp();
        cacheService.SetRawPrices(prices);

        profits = itemService.GetItemsProfit(prices, route);
        cacheService.SetItemsProfit(route, profits);

        return profits;
    }

    private Route GetRoute(int from, int to)
    {
        if (from == to)
        {
            throw new InvalidRouteException("From and To must be different");
        }

        var fromValid = Enum.IsDefined(typeof(CityEnum), from);
        var toValid = Enum.IsDefined(typeof(CityEnum), to);

        if (!fromValid || !toValid)
        {
            throw new KeyNotFoundException("Could not find these cities");
        }

        var fromCity = new City { Id = from, Name = Enum.GetName((CityEnum)from)! };
        var toCity = new City { Id = to, Name = Enum.GetName((CityEnum)to)! };

        return new Route { From = fromCity, To = toCity };
    }

    private async Task<IList<Price>> GetPricesFromHttp()
    {
        var http = httpFactory.CreateClient();
        var itemsQuery = itemService.GetItemQueryParams(albionData.MaxBactchSize);
        var tasks = new List<Task<List<Price>>>();

        Parallel.ForEach(itemsQuery, item =>
        {
            var endpoint = $"{albionData.BasePath}/{albionData.Prices}/{item}";
            tasks.Add(http.GetFromJsonAsync<List<Price>>(endpoint)!);
        });

        var prices = (await Task.WhenAll(tasks))
            .SelectMany(x => x)
            .ToList();

        return prices;
    }
}