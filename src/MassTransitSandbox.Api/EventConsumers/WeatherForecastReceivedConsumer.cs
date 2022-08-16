using MassTransitSandbox.App.Interfaces;

using MassTransit;

namespace MassTransitSandbox.Api.EventConsumers;

public class WeatherForecastReceivedConsumer : IConsumer<IWeatherForecastReceived>
{
    private readonly ILogger<WeatherForecastReceivedConsumer> _logger;

    public WeatherForecastReceivedConsumer(ILogger<WeatherForecastReceivedConsumer> logger)
    {
        _logger = logger;
    }
    /// <inheritdoc />
    public Task Consume(ConsumeContext<IWeatherForecastReceived> context)
    {
        _logger.LogDebug("WeatherForecast: {@WeatherForecast}", context.Message);

        return Task.CompletedTask;
    }
}
