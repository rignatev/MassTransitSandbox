using MassTransit;

using Microsoft.Extensions.Options;

using Shared.Contracts;

namespace ServiceB.Consumers;

public class ConsumerDefinitionBase<TConsumer> : ConsumerDefinition<TConsumer> where TConsumer : class, IConsumer
{
    private AppSettings AppSettings { get; }

    public ConsumerDefinitionBase(IOptions<AppSettings> appSettingsOptions) => AppSettings = appSettingsOptions.Value;

    /// <inheritdoc />
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator)
    {
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

        consumerConfigurator.UseConcurrentMessageLimit(AppSettings.ConcurrentMessageLimit);
        consumerConfigurator.UseConcurrencyLimit(AppSettings.ConcurrencyLimit);
    }
}
