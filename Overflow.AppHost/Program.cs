var builder = DistributedApplication.CreateBuilder(args);

var keyClock = builder.AddKeycloak("keycloak", 6001)
    .WithDataVolume("keycloak-data");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres-data")
    .WithPgAdmin();

var questionDb = postgres.AddDatabase("questionDb");

var questionService = builder.AddProject<Projects.QuestionService>("question-svc")
    .WithReference(keyClock)
    .WithReference(questionDb)
    .WaitFor(keyClock)
    .WaitFor(questionDb);

builder.Build().Run();