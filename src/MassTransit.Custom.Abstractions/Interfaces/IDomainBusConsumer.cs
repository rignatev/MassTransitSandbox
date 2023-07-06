namespace MassTransit.Custom.Abstractions.Interfaces;

public interface IDomainBusConsumer<in TMessage> : IConsumer<TMessage> where TMessage : class
{
}
