using MassTransit;

using MassTransitSandbox.App.Utils;

namespace MassTransitSandbox.Api.MassTransit.Filters;

/// <summary>
/// PublishFilter.
/// </summary>
/// <remarks>https://github.com/MassTransit/Sample-ScopedFilters</remarks>
public class LogMessagePublishFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly ILogger<LogMessagePublishFilter<T>> _logger;

    public LogMessagePublishFilter(ILogger<LogMessagePublishFilter<T>> logger) => _logger = logger;

    public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        _logger.LogInformation("Published message: {@Message}", context.Message);

        context.CorrelationId = Guid.Parse((ReadOnlySpan<char>)CorrelationIdAccessor.GetCorrelationId());

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}
