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
        //TODO: mover essa logica pra um middleware
        ValidCities(from, to);

        var http = httpFactory.CreateClient();
        var itemsQuery = itemService.GetItemQueryParams(albionData.MaxBactchSize);
        var tasks = new List<Task<List<Price>>>();

        foreach (string item in itemsQuery)
        {
            var endpoint = albionData.BasePath + albionData.Prices + item;
            tasks.Add(http.GetFromJsonAsync<List<Price>>(endpoint)!);
        }

        var response = (await Task.WhenAll(tasks))
            .SelectMany(x => x)
            .ToList();

        var profit = itemService.GetItemsProfit(response);
        return profit;
    }

    private void ValidCities(int from, int to)
    {
        var fromValid = Enum.IsDefined(typeof(City), from);
        var toValid = Enum.IsDefined(typeof(City), to);

        if (!fromValid || !toValid)
        {
            throw new KeyNotFoundException("Could not find these cities");
        }
    }
}