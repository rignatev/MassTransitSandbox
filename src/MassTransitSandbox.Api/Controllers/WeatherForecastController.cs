using MassTransitSandbox.App.Features;
using MassTransitSandbox.App.Models;

using MassTransitSandbox.Api.Models.WeatherForecast;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MassTransitSandbox.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISender _sender;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost(Name = "PublishWeatherForecast")]
    public async Task<ActionResult> Post(WeatherForecastPostRequest request)
    {
        await _sender.Send(new PublishWeatherForecastRequest(request.DaysCount));

        return Ok();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5)
            .Select(
                index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }
            )
            .ToArray();
    }
}
