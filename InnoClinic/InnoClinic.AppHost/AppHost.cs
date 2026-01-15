var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5435)
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var accountsDatabase = postgres.AddDatabase("innoclinic-accounts");

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-accounts", accountsDatabase)
    .WithReference(accountsDatabase)
    .WaitFor(accountsDatabase);

builder.Build().Run();
