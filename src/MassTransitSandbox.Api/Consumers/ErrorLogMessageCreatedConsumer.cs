using MassTransit;

using MassTransitSandbox.Api.Models;

using RabbitMQ.Client;

namespace MassTransitSandbox.Api.Consumers;

public class ErrorLogMessageCreatedConsumer : IConsumer<ILogMessageCreated>
{
    private readonly ILogger<ErrorLogMessageCreatedConsumer> _logger;

    public ErrorLogMessageCreatedConsumer(ILogger<ErrorLogMessageCreatedConsumer> logger) => _logger = logger;

    /// <inheritdoc />
    public Task Consume(ConsumeContext<ILogMessageCreated> context)
    {
        _logger.LogInformation("Error: {Message}", context.Message.Message);

        return Task.CompletedTask;
    }
}

public class ErrorLogMessageCreatedConsumerDefinition : ConsumerDefinition<ErrorLogMessageCreatedConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ErrorLogMessageCreatedConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<ILogMessageCreated>(
                x =>
                {
                    x.RoutingKey = "#.error.#";
                    x.ExchangeType = ExchangeType.Topic;
                }
            );
        }
    }
}
