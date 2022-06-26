using Microsoft.AspNetCore.Mvc;
using AlbionRoad.Application.Handlers;

namespace AlbionRoad.API.Controllers;

[ApiController]
[Route("api/v2/route")]
public class RouteController : ControllerBase
{
    private readonly TravelHandler travelHandler;

    public RouteController(TravelHandler travelHandler)
    {
        this.travelHandler = travelHandler;
    }

    [HttpGet("travel/from/{from}/to/{to}")]
    public async Task<IActionResult> Travel(int from, int to)
    {
        //TODO: Middleware para filtrar id das cidades antes de entrar no handler
        var result = await travelHandler.Travel(from, to);
        return Ok(result);
    }
}