var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres").WithPgAdmin();

var portfolioDb = db.AddDatabase("portfolio-db", "portfolio");
var usersApi = builder.AddDotnetProject("portfolio-api", "../Apex.Portfolio.Api/Apex.Portfolio.Api.csproj")
    .WithReference(portfolioDb, connectionName: "DefaultConnection")
    .WithHttpHealthCheck("/health");

var userDb = db.AddDatabase("users-db", "users");
var portfolioApi = builder.AddDotnetProject("users-api", "../Apex.Users.Api/Apex.Users.Api.csproj")
    .WithReference(userDb, connectionName: "DefaultConnection")
    .WithHttpHealthCheck("/health");

builder.AddDotnetProject("gateway", "../Apex.Gateway/Apex.Gateway.csproj")
    .WithReference(usersApi)
    .WithReference(portfolioApi)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
