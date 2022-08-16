using MassTransitSandbox.Api.Middlewares;

namespace MassTransitSandbox.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRequestLogContext(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<RequestLogContextMiddleware>();

        return applicationBuilder;
    }
}
