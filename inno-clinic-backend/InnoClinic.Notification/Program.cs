using InnoClinic.Notification;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMQClient("rabbitmq");
builder.AddMongoDBClient("notifications-db");
builder.Services.AddWebHostServices(builder.Configuration);

var app = builder.Build();
await app.RunAsync();