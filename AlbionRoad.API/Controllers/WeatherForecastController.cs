using Microsoft.AspNetCore.Mvc;
using AlbionRoad.Domain.Models;
using System.Text.Json;

namespace AlbionRoad.API.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("teste")]
    public IList<Item> Get()
    {
        //C:\Users\mathe\Projects\albion-road\api\AlbionRoad.Resources\items.json
        StreamReader r = new StreamReader("../AlbionRoad.Resources/items.json");
        string toJson = r.ReadToEnd();
        IList<Item> items = JsonSerializer.Deserialize<List<Item>>(toJson);
        return items;
    }
}
