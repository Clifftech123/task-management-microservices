using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);



var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("eventbus");
var postgres = builder.AddPostgres("postgres");



// database 
var userSereviceDb = builder.AddPostgres("userservicedb");
var projectServiceDb = builder.AddPostgres("projectservicedb");
var taskServiceDb = builder.AddPostgres("taskservicedb");


// services
var projectserviceApi = builder.AddProject<Projects.ProjectService_API>("projectservice-api")
     .WithReference(projectServiceDb)
     .WithReference(rabbitMq)
     .WithReference(redis);
      
var taskServiceApi = builder.AddProject<Projects.TaskService_API>("taskservice-api")
    .WithReference(taskServiceDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

var userServiceApi = builder.AddProject<Projects.UserService_API>("userservice-api")
     .WithReference(userSereviceDb)
     .WithReference(rabbitMq);
     


// // Reverse proxies
builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WithReference(projectserviceApi)
    .WithReference(taskServiceApi)
    .WithReference(userServiceApi);




builder.Build().Run();
