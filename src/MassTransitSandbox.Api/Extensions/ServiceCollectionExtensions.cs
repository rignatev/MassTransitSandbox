using MassTransit;

using MassTransitSandbox.Api.EventConsumers;
using MassTransitSandbox.Api.MassTransit.Filters;
using MassTransitSandbox.Api.Models;

namespace MassTransitSandbox.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransitCustom(this IServiceCollection services)
    {
        services.AddMassTransit(
            x =>
            {
                x.AddConsumer<WeatherForecastReceivedConsumer>();

                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.Host(
                            "localhost",
                            "/",
                            h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            }
                        );

                        cfg.UseConsumeFilter(typeof(LogMessageConsumeFilter<>), context);
                        cfg.UsePublishFilter(typeof(LogMessagePublishFilter<>), context);
                        cfg.ConfigureEndpoints(context);
                    }
                );
            }
        );

        return services;
    }
}
