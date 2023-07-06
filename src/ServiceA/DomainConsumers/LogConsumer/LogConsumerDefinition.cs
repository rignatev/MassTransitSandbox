using MassTransit;

namespace ServiceA.DomainConsumers.LogConsumer;

public class LogConsumerDefinition : ConsumerDefinition<LogConsumer>
{
    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<LogConsumer> consumerConfigurator)
    {
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

        consumerConfigurator.UseMessageRetry(
            x =>
            {
                x.Handle<Exception>();
                x.Immediate(retryLimit: 10);
            }
        );
    }
}
