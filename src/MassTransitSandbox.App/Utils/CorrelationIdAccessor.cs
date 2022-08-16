namespace MassTransitSandbox.App.Utils;

public static class CorrelationIdAccessor
{
    private static readonly AsyncLocal<string?> CorrelationId = new();

    public static void SetCorrelationId(string? correlationId)
    {
        CorrelationId.Value = correlationId;
    }

    public static string? GetCorrelationId() => CorrelationId.Value;
}
