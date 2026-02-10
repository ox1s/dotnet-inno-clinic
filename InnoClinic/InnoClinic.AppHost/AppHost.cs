var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("inno-clinic-docker");

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
    .WithEnvironment("WebAppUrl", "http://localhost:7778");

var appointmentApi = builder.AddProject<Projects.Appointment_Api>("appointment-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)
    .WithReference(sharedDatabase)
    .WaitFor(sharedDatabase)/* 
 */    .WithEnvironment("AppUrl", "https://localhost:7779")
    .WithEnvironment("WebAppUrl", "http://localhost:7780");

builder.Build().Run();