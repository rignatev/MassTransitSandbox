using MassTransit;

using MassTransitSandbox.App.Interfaces;
using MassTransitSandbox.App.Models;
using MassTransitSandbox.App.Utils;

using MediatR;

namespace MassTransitSandbox.App.Features;

public class PublishWeatherForecastRequestHandler : IRequestHandler<PublishWeatherForecastRequest>
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IBus _bus;

    public PublishWeatherForecastRequestHandler(IBus bus)
    {
        _bus = bus;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(PublishWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        Task[] tasks = Enumerable.Range(start: 1, request.DaysCount)
            .Select(
                index =>
                {
                    var message = new WeatherForecastReceived
                    {
                        CorrelationId = CorrelationIdAccessor.GetCorrelationId(),
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    };

                    return _bus.Publish<IWeatherForecastReceived>(message, cancellationToken);
                }
            )
            .ToArray();

        await Task.WhenAll(tasks);

        return Unit.Value;
    }
}
