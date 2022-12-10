namespace MassTransitSandbox.Api.Models;

public class LogMessageCreated : ILogMessageCreated
{
    /// <inheritdoc />
    public string Message { get; set; } = null!;
}
