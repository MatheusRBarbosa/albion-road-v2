using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

using AlbionRoad.Domain.Models;
using AlbionRoad.Domain.Enums;
using AlbionRoad.Domain.Interfaces.Services;
using AlbionRoad.Resources.Configs;

namespace AlbionRoad.Application.Handlers;

public class TravelHandler
{
    private IMapper mapper;
    private AlbionData albionData;
    private IHttpClientFactory httpFactory;
    private IItemService itemService;

    public TravelHandler(
        IMapper mapper,
        IOptions<AlbionData> albionData,
        IHttpClientFactory httpFactory,
        IItemService itemService
    )
    {
        this.mapper = mapper;
        this.albionData = albionData.Value;
        this.httpFactory = httpFactory;
        this.itemService = itemService;
    }

    public async Task<IList<Profit>> Travel(int from, int to)
    {
        var route = GetRoute(from, to);

        var http = httpFactory.CreateClient();
        var itemsQuery = itemService.GetItemQueryParams(albionData.MaxBactchSize);
        var tasks = new List<Task<List<Price>>>();

        Parallel.ForEach(itemsQuery, item =>
        {
            var endpoint = $"{albionData.BasePath}/{albionData.Prices}/{item}?{route.ToUrlQueryParam}";
            tasks.Add(http.GetFromJsonAsync<List<Price>>(endpoint)!);
        });

        var response = (await Task.WhenAll(tasks))
            .SelectMany(x => x)
            .ToList();

        var profit = itemService.GetItemsProfit(response, route);
        return profit;
    }

    private Route GetRoute(int from, int to)
    {
        var fromValid = Enum.IsDefined(typeof(CityEnum), from);
        var toValid = Enum.IsDefined(typeof(CityEnum), to);

        if (!fromValid || !toValid)
        {
            //TODO: ExceptionFilter
            throw new KeyNotFoundException("Could not find these cities");
        }

        var fromCity = new City { Id = from, Name = Enum.GetName((CityEnum)from)! };
        var toCity = new City { Id = to, Name = Enum.GetName((CityEnum)to)! };

        return new Route { From = fromCity, To = toCity };
    }
}