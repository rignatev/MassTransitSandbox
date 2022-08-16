using MassTransit;

using MassTransitSandbox.App.Utils;

namespace MassTransitSandbox.Api.MassTransit.Filters;

/// <summary>
/// ConsumeFilter.
/// </summary>
/// <remarks>https://github.com/MassTransit/Sample-ScopedFilters</remarks>
public class LogMessageConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly ILogger<LogMessageConsumeFilter<T>> _logger;

    public LogMessageConsumeFilter(ILogger<LogMessageConsumeFilter<T>> logger) => _logger = logger;

    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        CorrelationIdAccessor.SetCorrelationId(context.CorrelationId.ToString());

        using var loggerBeginScope = _logger.BeginScope(
            new Dictionary<string, object?> { { "CorrelationId", CorrelationIdAccessor.GetCorrelationId() } }
        );

        _logger.LogInformation("Consumed message: {@Message}", context.Message);
            
        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}
