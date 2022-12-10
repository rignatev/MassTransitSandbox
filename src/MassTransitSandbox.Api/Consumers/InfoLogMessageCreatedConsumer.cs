using MassTransit;

using MassTransitSandbox.Api.Models;

using RabbitMQ.Client;

namespace MassTransitSandbox.Api.Consumers;

public class InfoLogMessageCreatedConsumer : IConsumer<ILogMessageCreated>
{
    private readonly ILogger<InfoLogMessageCreatedConsumer> _logger;

    public InfoLogMessageCreatedConsumer(ILogger<InfoLogMessageCreatedConsumer> logger) => _logger = logger;

    /// <inheritdoc />
    public Task Consume(ConsumeContext<ILogMessageCreated> context)
    {
        _logger.LogInformation("Info: {Message}", context.Message.Message);

        return Task.CompletedTask;
    }
}

public class InfoLogMessageCreatedConsumerDefinition : ConsumerDefinition<InfoLogMessageCreatedConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<InfoLogMessageCreatedConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind<ILogMessageCreated>(
                x =>
                {
                    x.RoutingKey = "#.info.#";
                    x.ExchangeType = ExchangeType.Topic;
                }
            );
        }
    }
}
