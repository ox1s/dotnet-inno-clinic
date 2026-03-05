
var builder = DistributedApplication.CreateBuilder(args);
const string profileInternalSecret = "dev-internal-secret";

var compose = builder.AddDockerComposeEnvironment("inno-clinic-docker");

var mailpit = builder.AddMailPit("mailpit");

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
                    .WithManagementPlugin();

var mongo = builder.AddMongoDB("mongo")
                .WithMongoExpress();

var mongodb = mongo.AddDatabase("notifications-db");

var notificationsApi = builder.AddProject<Projects.InnoClinic_Notification>("notifications-api")
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

    .WaitFor(mongodb)
    .WithReference(mongodb)

    .WaitFor(mailpit)
    .WithReference(mailpit);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sharedDatabase = postgres.AddDatabase("innoclinic-database");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

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
    .WithEnvironment("InternalAuth__SharedSecret", profileInternalSecret)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

    .WaitFor(mongodb)
    .WithReference(mongodb)

    .WithEnvironment("AppUrl", "https://localhost:7123")
    .WithEnvironment("WebAppUrl", "http://localhost:7124");

identityApi
    .WithReference(profileApi)
    .WaitFor(profileApi);


var gateway = builder.AddProject<Projects.Gateway_Api>("gateway")
    .WithReference(identityApi)
    .WithReference(appointmentApi)
    .WithReference(clinicManagementApi)
    .WithReference(profileApi)

    .WaitFor(identityApi)
    .WaitFor(appointmentApi)
    .WaitFor(clinicManagementApi)
    .WaitFor(profileApi)

    .WithExternalHttpEndpoints();

builder.Build().Run();