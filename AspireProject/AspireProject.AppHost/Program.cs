var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var dbName = "Todos";
var postgresdb = builder.AddPostgres("pg")
                .WithEnvironment("POSTGRES_DB", dbName)
                .WithBindMount("../AspireProject.ApiService/data/postgres", "/docker-entrypoint-initdb.d")
                .AddDatabase(dbName);



var apiService = builder.AddProject<Projects.AspireProject_ApiService>("apiservice")
                .WithReference(postgresdb);

builder.AddProject<Projects.AspireProject_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReference(postgresdb);
    
builder.Build().Run();
