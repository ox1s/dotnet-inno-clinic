using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Profile.Api.Common;
using Profile.Api.Endpoints;
using Profile.Api.Extensions;
using Profile.Features.Doctors.EditDoctorStatusByBot;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using Profile.Infrastructure;
using Profile.Infrastructure.Services;

using Quartz;

using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;
var host = builder.Host;

services.AddEndpointsApiExplorer();
services.AddHttpClient();
services.AddEndpoints();

builder.AddMongoDBClient("notifications-db");

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("StatusJob");

    q.AddJob<EmailReminderJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("TelegramStatusJob-trigger")
        // .WithCronSchedule("0 * * ? * *")
        .WithCronSchedule("0 0 16 ? * MON-FRI")
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

host.UseWolverine(opts =>
    {
        var rabbitConnectionString = builder.Configuration.GetConnectionString("rabbitmq");

        if (rabbitConnectionString != null)
        {
            opts.UseRabbitMq(new Uri(rabbitConnectionString))
                .AutoProvision();
        }
        opts.PublishMessage<DoctorCreated>()
            .ToRabbitExchange("doctor-created-events");

        opts.PublishMessage<SendDailyPollCommand>()
            .ToRabbitExchange("notifications");

        opts.PublishMessage<TelegramAccountLinked>()
            .ToRabbitExchange("telegram-account-linked-events");

        opts.Discovery.IncludeAssembly(typeof(CreateDoctorProfileCommandHandler).Assembly);
        opts.Discovery.IncludeAssembly(typeof(EmailReminderJob).Assembly);
        opts.Discovery.IncludeAssembly(typeof(LinkTelegramAccountCommandHandler).Assembly);
    }
);

services.AddHealthChecks();

services
        .AddWebHostInfrastructure(configuration, environment)
        .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.InitializeAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.MapDefaultEndpoints();

await app.RunAsync();

public partial class Program { }
