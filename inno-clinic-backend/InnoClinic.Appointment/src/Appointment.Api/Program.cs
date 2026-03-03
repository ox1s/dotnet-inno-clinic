using Appointment.Api.Data;
using Appointment.Api.Endpoints;
using Appointment.Api.Extensions;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{

    var services = builder.Services;
    var configuration = builder.Configuration;
    var environment = builder.Environment;
    var logging = builder.Logging;

    services.AddEndpointsApiExplorer();
    services.AddHttpClient();

    logging.AddConsole();

    services.AddWebHostInfrastructure(configuration, environment);

    services.AddEndpoints();

    services.AddValidatorsFromAssembly(typeof(Program).Assembly);
}
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider
            .GetRequiredService<AppointmentDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapEndpoints();

    app.Run();
}

public partial class Program { }