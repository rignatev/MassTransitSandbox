using Microsoft.Extensions.Primitives;

namespace MassTransitSandbox.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetCorrelationId(this HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out StringValues correlationIds);

        string? correlationId = correlationIds.FirstOrDefault();
        if (correlationId == null)
        {
            return null;
        }

        return Guid.Parse(correlationId);
    }
}
