using MassTransitSandbox.App.Interfaces;

namespace MassTransitSandbox.App.Models;

public class WeatherForecastReceived : IWeatherForecastReceived
{
    /// <inheritdoc />
    public string? CorrelationId { get; set; }

    /// <inheritdoc />
    public DateTime Date { get; set; }

    /// <inheritdoc />
    public int TemperatureC { get; set; }

    /// <inheritdoc />
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <inheritdoc />
    public string? Summary { get; set; }
}
