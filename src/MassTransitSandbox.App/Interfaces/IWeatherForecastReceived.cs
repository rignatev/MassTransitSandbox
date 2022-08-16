namespace MassTransitSandbox.App.Interfaces;

public interface IWeatherForecastReceived
{
    string? CorrelationId { get; set; }

    DateTime Date { get; set; }

    int TemperatureC { get; set; }

    int TemperatureF { get; }

    string? Summary { get; set; }
}
