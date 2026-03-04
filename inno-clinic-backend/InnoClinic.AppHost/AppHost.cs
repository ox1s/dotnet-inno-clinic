
var builder = DistributedApplication.CreateBuilder(args);

var compose = builder.AddDockerComposeEnvironment("inno-clinic-docker");

var mailpit = builder.AddMailPit("mailpit");

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sharedDatabase = postgres.AddDatabase("innoclinic-database");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(mailpit)
    .WithReference(mailpit)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WithEnvironment("AppUrl", "https://localhost:7777")
    .WithEnvironment("WebAppUrl", "http://localhost:7778");

var appointmentApi = builder.AddProject<Projects.Appointment_Api>("appointment-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WithEnvironment("AppUrl", "https://localhost:7779")
    .WithEnvironment("WebAppUrl", "http://localhost:7780");

var clinicManagementApi = builder.AddProject<Projects.ClinicManagement_Api>("clinic-management-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WithEnvironment("AppUrl", "https://localhost:7113")
    .WithEnvironment("WebAppUrl", "http://localhost:7114");

var profileApi = builder.AddProject<Projects.Profile_Api>("profile-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WithEnvironment("AppUrl", "https://localhost:7123")
    .WithEnvironment("WebAppUrl", "http://localhost:7124");

// builder.AddViteApp("frontend", "../inno-clinic-frontend")
//     .WithHttpEndpoint(env: "PORT")
//     .WithReference(identityApi)
//     .WithReference(appointmentApi)
//     .WithReference(clinicManagementApi);

builder.Build().Run();