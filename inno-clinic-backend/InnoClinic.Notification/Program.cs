using System.Text.Json;

using Serilog;

using InnoClinic.Notification;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var services = builder.Services;

builder.AddRabbitMQClient("rabbitmq");
builder.AddMongoDBClient("notifications-db");
services.AddWebHostServices(builder.Configuration);
host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);

services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

var app = builder.Build();

app.UseSerilogRequestLogging();

await app.RunAsync();