using MediatR;

namespace MassTransitSandbox.App.Features;

public record PublishWeatherForecastRequest(int DaysCount) : IRequest;

