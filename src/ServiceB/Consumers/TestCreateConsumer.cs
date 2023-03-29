using MassTransit;

using Shared.Contracts.Models.Rpc.ServiceB.TestCreate;

namespace ServiceB.Consumers;

public class TestCreateConsumer : IConsumer<ServiceBTestCreateRequest>
{
    private readonly ILogger<TestCreateConsumer> _logger;

    public TestCreateConsumer(ILogger<TestCreateConsumer> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Consume(ConsumeContext<ServiceBTestCreateRequest> context)
    {
        _logger.LogDebug("Message name: {Name}", context.Message.Name);

        await context.RespondAsync(new ServiceBTestCreateResponse { Id = context.Message.Name.GetHashCode() });
    }
}
