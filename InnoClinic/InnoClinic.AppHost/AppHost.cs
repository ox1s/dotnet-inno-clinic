var builder = DistributedApplication.CreateBuilder(args);


var mailpit = builder.AddMailPit("mailpit");

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sharedDatabase = postgres.AddDatabase("innoclinic-database");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)
    .WithReference(mailpit)
    .WithReference(sharedDatabase)
    .WaitFor(sharedDatabase)
    .WithEnvironment("AppUrl", "https://localhost:7777")
    .WithEnvironment("WebAppUrl", "http://localhost:6666");

var appointmentApi = builder.AddProject<Projects.Appointment_Api>("appointment-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)
    .WithReference(sharedDatabase)
    .WaitFor(sharedDatabase)
    .WithEnvironment("AppUrl", "https://localhost:8888")
    .WithEnvironment("WebAppUrl", "http://localhost:7777");

builder.Build().Run();
