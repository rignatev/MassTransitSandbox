using MassTransit;
using MassTransit.Custom.Abstractions.Interfaces;

namespace ServiceA.DomainConsumers.LogConsumer;

public class LogConsumer : IDomainBusConsumer<LogTextReceived>
{
    private readonly ILogger<LogConsumer> _logger;

    public LogConsumer(ILogger<LogConsumer> logger) => _logger = logger;

    /// <inheritdoc />
    public Task Consume(ConsumeContext<LogTextReceived> context)
    {
        _logger.LogInformation("{Text}", context.Message.Text);

        return Task.CompletedTask;
    }
}
