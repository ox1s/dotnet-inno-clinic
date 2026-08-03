
var builder = DistributedApplication.CreateBuilder(args);

// config -----------------------------------------------------
var botApiKey = builder.AddParameter("botApiKey", secret: true);
var telegramToken = builder.AddParameter("telegramToken", secret: true);
// ------------------------------------------------------------
///////////////////////////////////////////////////////////////
// services ---------------------------------------------------
var mailpit = builder.AddMailPit("mailpit");
var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();

var minio = builder.AddContainer("minio", "minio/minio")
    .WithHttpEndpoint(port: 9000, targetPort: 9000, name: "api")
    .WithHttpEndpoint(port: 9001, targetPort: 9001, name: "console")
    .WithEnvironment("MINIO_ROOT_USER", "minioAdmin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "minioAdmin")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithBindMount("minio-data", "/data")
    .WithLifetime(ContainerLifetime.Persistent);
// ------------------------------------------------------------
///////////////////////////////////////////////////////////////
// databases --------------------------------------------------
var mongo = builder.AddMongoDB("mongo").WithMongoExpress().WithLifetime(ContainerLifetime.Persistent);
var mongodb = mongo.AddDatabase("notifications-db");

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var sharedDatabase = postgres.AddDatabase("innoclinic-database");
// ------------------------------------------------------------
///////////////////////////////////////////////////////////////
// projects ---------------------------------------------------
var notificationsApi = builder.AddProject<Projects.InnoClinic_Notification>("notifications-api")
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

    .WaitFor(mongodb)
    .WithReference(mongodb)

    .WaitFor(mailpit)
    .WithReference(mailpit);

var profileApi = builder.AddProject<Projects.Profile_Api>("profile-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

    .WaitFor(mongodb)
    .WithReference(mongodb)

    .WithEnvironment("AppUrl", "https://localhost:7123")
    .WithEnvironment("WebAppUrl", "http://localhost:7124");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)

    .WaitFor(profileApi)
    .WithReference(profileApi)

    .WithEnvironment("AppUrl", "https://localhost:7777")
    .WithEnvironment("WebAppUrl", "http://localhost:7778");

var clinicManagementApi = builder.AddProject<Projects.ClinicManagement_Api>("clinic-management-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(minio)
    .WithEnvironment("ConnectionStrings__minio", "Endpoint=http://localhost:9000;AccessKey=minioAdmin;SecretKey=minioAdmin")

    .WaitFor(profileApi)
    .WithReference(profileApi)

    .WithEnvironment("AppUrl", "https://localhost:7113")
    .WithEnvironment("WebAppUrl", "http://localhost:7114");

var appointmentApi = builder.AddProject<Projects.Appointment_Api>("appointment-api")
    .WithEnvironment("ConnectionStrings__innoclinic-database", sharedDatabase)

    .WaitFor(sharedDatabase)
    .WithReference(sharedDatabase)

    .WaitFor(clinicManagementApi)
    .WithReference(clinicManagementApi)

    .WaitFor(profileApi)
    .WithReference(profileApi)

    .WaitFor(identityApi)
    .WithReference(identityApi)

    .WithEnvironment("AppUrl", "https://localhost:7779")
    .WithEnvironment("WebAppUrl", "http://localhost:7780");

var _ = builder.AddPythonApp("telegram-bot", "../InnoClinic.TelegramBot", "bot.py")
    .WithEnvironment("TELEGRAM_BOT_TOKEN", telegramToken)
    .WithEnvironment("API_KEY", botApiKey)
    .WithEnvironment("BACKEND_API_URL", profileApi.GetEndpoint("https"))
    .WaitFor(profileApi)
    .WaitFor(notificationsApi);
// ------------------------------------------------------------
///////////////////////////////////////////////////////////////
// gateway ----------------------------------------------------
var gateway = builder.AddProject<Projects.Gateway_Api>("gateway")
    .WithReference(identityApi)
    .WithReference(appointmentApi)
    .WithReference(clinicManagementApi)
    .WithReference(profileApi)
    .WithReference(notificationsApi)

    .WaitFor(identityApi)
    .WaitFor(appointmentApi)
    .WaitFor(clinicManagementApi)
    .WaitFor(profileApi)
    .WaitFor(notificationsApi)

    .WithExternalHttpEndpoints();

// ------------------------------------------------------------
///////////////////////////////////////////////////////////////
// frontend ---------------------------------------------------
builder.AddViteApp("client-app", "../../inno-clinic-frontend")
   .WithReference(gateway)
   .WithEnvironment("VITE_API_BASE_URL", gateway.GetEndpoint("https"))
   .WithViteConfig("./vite.config.js")

   .WaitFor(gateway);

await builder.Build().RunAsync();