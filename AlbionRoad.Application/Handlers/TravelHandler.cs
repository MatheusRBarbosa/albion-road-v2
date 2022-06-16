using AutoMapper;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Net.Http.Json;

using AlbionRoad.Domain.Models;
using AlbionRoad.Resources.Configs;

namespace AlbionRoad.Application.Handlers;

public class TravelHandler
{
    private IMapper mapper;
    private AlbionData albionData;
    private IHttpClientFactory httpFactory;

    public TravelHandler(
        IMapper mapper,
        IOptions<AlbionData> albionData,
        IHttpClientFactory httpFactory
    )
    {
        this.mapper = mapper;
        this.albionData = albionData.Value;
        this.httpFactory = httpFactory;
    }

    public async Task<dynamic> Travel()
    {
        var http = httpFactory.CreateClient();
        var endpoint = albionData.BasePath + albionData.Prices + "T4_BAG";

        var response = await http.GetFromJsonAsync<IList<Price>>(endpoint);

        return response!;
    }
}