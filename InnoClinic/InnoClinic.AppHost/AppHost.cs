var builder = DistributedApplication.CreateBuilder(args);


var mailpit = builder.AddMailPit("mailpit");

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var accountsDatabase = postgres.AddDatabase("innoclinic-accounts");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-accounts", accountsDatabase)
    .WithReference(mailpit)
    .WithReference(accountsDatabase)
    .WaitFor(accountsDatabase)
    .WithEnvironment("AppUrl", "https://localhost:7777")
    .WithEnvironment("WebAppUrl", "http://localhost:6666");

builder.Build().Run();
