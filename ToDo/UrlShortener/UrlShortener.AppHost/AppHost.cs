var builder = DistributedApplication.CreateBuilder(args);

// Database containers
var linkDb = builder
    .AddContainer("linkdb", "scylladb/scylla");
    // .WithVolumeMount("./volumes/linkdb", "/var/lib/scylla")
    // .WithEndpoint(name: "cql", port: 9042, scheme: "tcp")
    // .WithEndpoint(name: "thrift", port: 9160, scheme: "tcp")
    // .WithEndpoint(name: "rest", port: 10000, scheme: "http");

var userDb = builder
    .AddPostgres("userdb")
    .AddDatabase("usersdb");

// KeyCloak for authentication
var authApi = builder
    .AddContainer("keycloak", "quay.io/keycloak/keycloak:latest")
    .WithArgs("start-dev")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithEndpoint(name: "http", port: 8080, targetPort: 8081, scheme: "http");

// Read API
var readApi = builder
    .AddProject<Projects.UrlShortener_ReadApi>("readapi");
    // .WithEnvironment("ScyllaDb__ConnectionString", linkDb.GetConnectionString("cql"));

// Write API
    var writeApi = builder
        .AddProject<Projects.UrlShortener_WriteApi>("writeapi");
    // .WithEnvironment("ScyllaDb__ConnectionString", linkDb.GetConnectionString("cql"));

// Web Frontend
var webApp = builder
    .AddProject<Projects.UrlShortener_Web>("webapp")
    .WithReference(readApi)
    .WithReference(writeApi)
    .WithEnvironment("AuthApi__Url", authApi.GetEndpoint("http"));

builder.Build().Run();