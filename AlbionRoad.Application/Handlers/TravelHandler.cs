using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

using AlbionRoad.Domain.Models;
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

    public async Task<dynamic> Travel()
    {
        var http = httpFactory.CreateClient();
        var itemsQuery = itemService.GetItemQueryParams(albionData.MaxBactchSize);

        //TODO: Pra cada item em itemsQuery disparar uma request assincrona.
        //Nao eh preciso uma request esperar a outra, eu so preciso do resultado de todas

        // var endpoint = albionData.BasePath + albionData.Prices + itemsQuery;
        // var response = await http.GetFromJsonAsync<IList<Price>>(endpoint);

        return response!;
    }
}