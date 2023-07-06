namespace MassTransit.Custom.Abstractions.Interfaces;

public interface IBusConsumer<in TMessage> : IConsumer<TMessage> where TMessage : class
{
}
