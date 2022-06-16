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

    [HttpGet("travel")]
    public async Task<IActionResult> Travel()
    {
        var result = await travelHandler.Travel();
        return Ok(result);
    }
}