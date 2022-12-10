using System.Reflection;

using MassTransitSandbox.Api.Models;

using MassTransit;

using RabbitMQ.Client;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MassTransitHostOptions>(
    options =>
    {
        options.WaitUntilStarted = true;
        options.StartTimeout = TimeSpan.FromSeconds(30);
        options.StopTimeout = TimeSpan.FromMinutes(1);
    }
);

builder.Services.AddMassTransit(
    busRegistrationConfigurator =>
    {
        busRegistrationConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(includeNamespace: true));

        var entryAssembly = Assembly.GetEntryAssembly();
        busRegistrationConfigurator.AddConsumers(entryAssembly);
        // busRegistrationConfigurator.AddConsumer<InfoLogMessageCreatedConsumer>(typeof(InfoLogMessageCreatedConsumerDefinition));
        // busRegistrationConfigurator.AddConsumer<ErrorLogMessageCreatedConsumer>(typeof(ErrorLogMessageCreatedConsumerDefinition));

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

                // Настройки публикации сообщения. Тип обменника должен быть таким же как и в настройках консьюмеров, которые его создают.
                cfg.Publish<ILogMessageCreated>(x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });

                cfg.ConfigureEndpoints(context);
            }
        );
    }
);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
