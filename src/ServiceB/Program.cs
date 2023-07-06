using System.Reflection;

using MassTransit;

using Serilog;

using Shared.Contracts;

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

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

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
