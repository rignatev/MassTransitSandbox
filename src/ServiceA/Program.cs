using System.Reflection;

using MassTransit;
using MassTransit.Custom.Abstractions.Interfaces;

using Serilog;

using ServiceA.Utils;

using Shared.Contracts.Models.Rpc.ServiceB.TestCreate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Seq("http://localhost:5341");
    }
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(
    busRegistrationConfigurator =>
    {
        busRegistrationConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(includeNamespace: true));

        var busConsumerTypes = Assembly.GetEntryAssembly()!.GetAllImplementations(typeof(IBusConsumer<>)).ToArray();

        busRegistrationConfigurator.AddConsumers(busConsumerTypes);
        // busRegistrationConfigurator.AddConsumer<InfoLogMessageCreatedConsumer>(typeof(InfoLogMessageCreatedConsumerDefinition));
        // busRegistrationConfigurator.AddConsumer<ErrorLogMessageCreatedConsumer>(typeof(ErrorLogMessageCreatedConsumerDefinition));

        busRegistrationConfigurator.AddRequestClient<ServiceBTestCreateRequest>();

        busRegistrationConfigurator.UsingRabbitMq(
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

                cfg.ConfigureEndpoints(context);
            }
        );
    }
);

builder.Services.AddMassTransit<IDomainBus>(
    busRegistrationConfigurator =>
    {
        busRegistrationConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(includeNamespace: true));

        var busConsumerTypes = Assembly.GetEntryAssembly()!.GetAllImplementations(typeof(IDomainBusConsumer<>)).ToArray();

        busRegistrationConfigurator.AddConsumers(busConsumerTypes);
        // busRegistrationConfigurator.AddConsumer<LogConsumer>(typeof(LogConsumerDefinition));

        busRegistrationConfigurator.UsingRabbitMq(
            (context, cfg) =>
            {
                cfg.Host(
                    "localhost",
                    "ServiceA",
                    h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    }
                );

                cfg.ConfigureEndpoints(context);
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
