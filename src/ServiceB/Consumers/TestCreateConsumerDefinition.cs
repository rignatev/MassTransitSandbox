using MassTransit;

using Microsoft.Extensions.Options;

using Shared.Contracts;

namespace ServiceB.Consumers;

public class TestCreateConsumerDefinition : ConsumerDefinitionBase<TestCreateConsumer>
{
    /// <inheritdoc />
    public TestCreateConsumerDefinition(IOptions<AppSettings> appSettingsOptions) : base(appSettingsOptions)
    {
    }

    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TestCreateConsumer> consumerConfigurator)
    {
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

        consumerConfigurator.UseConcurrencyLimit(concurrencyLimit: 2);
        consumerConfigurator.UseMessageRetry(
            x =>
            {
                x.Immediate(retryLimit: 10);
                x.Handle<Exception>();
            }
        );
    }
}
