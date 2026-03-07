
var builder = DistributedApplication.CreateBuilder(args);

var mailpit = builder.AddMailPit("mailpit");

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
                    .WithManagementPlugin();

var botApiKey = builder.Configuration["BotSettings:ApiKey"];
var telegramToken = builder.Configuration["BotSettings:BotToken"];

var mongo = builder.AddMongoDB("mongo")
                .WithMongoExpress()
                .WithLifetime(ContainerLifetime.Persistent);

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


var telegramBot = builder.AddPythonApp("telegram-bot", "../InnoClinic.TelegramBot", "bot.py")
    .WithEnvironment("TELEGRAM_BOT_TOKEN", telegramToken)
    .WithEnvironment("API_KEY", botApiKey)
    .WithEnvironment("BACKEND_API_URL", profileApi.GetEndpoint("https"))
    .WaitFor(profileApi);

await builder.Build().RunAsync();