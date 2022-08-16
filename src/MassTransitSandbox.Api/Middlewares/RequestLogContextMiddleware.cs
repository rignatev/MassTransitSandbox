using MassTransitSandbox.Api.Extensions;
using MassTransitSandbox.App.Utils;

namespace MassTransitSandbox.Api.Middlewares;

public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogContextMiddleware> _logger;

    public RequestLogContextMiddleware(RequestDelegate next, ILogger<RequestLogContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public Task Invoke(HttpContext context)
    {
        Guid correlationId = context.GetCorrelationId() ?? Guid.NewGuid();

        CorrelationIdAccessor.SetCorrelationId(correlationId.ToString());

        using (_logger.BeginScope(new Dictionary<string, object?> { { "CorrelationId", CorrelationIdAccessor.GetCorrelationId() } }))
        {
            return _next.Invoke(context);
        }
    }
}
