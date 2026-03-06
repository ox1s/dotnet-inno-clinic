using System.Text.Json;

using InnoClinic.Notification;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMQClient("rabbitmq");
builder.AddMongoDBClient("notifications-db");
builder.Services.AddWebHostServices(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

var app = builder.Build();
await app.RunAsync();