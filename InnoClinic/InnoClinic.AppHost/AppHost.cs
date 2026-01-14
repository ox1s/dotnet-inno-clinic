var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> database = builder
    .AddPostgres("database")
    .WithImage("postgres:17")
    .AddDatabase("innoclinic-accounts");

builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithEnvironment("ConnectionStrings__innoclinic-accounts", database)
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
