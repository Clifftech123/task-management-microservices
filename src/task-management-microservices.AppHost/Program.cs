var builder = DistributedApplication.CreateBuilder(args);


var seq = builder.AddSeq("seq");


var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("eventbus");

// services
var projectserviceApi = builder.AddProject<Projects.ProjectService_API>("projectservice-api")

     .WithReference(rabbitMq)
     .WithReference(redis)
     .WithReference(seq);

var taskServiceApi = builder.AddProject<Projects.TaskService_API>("taskservice-api")

    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(seq);

var userServiceApi = builder.AddProject<Projects.UserService_API>("userservice-api")

     .WithReference(rabbitMq)
     .WithReference(redis);



// // Reverse proxies
builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WithReference(projectserviceApi)
    .WithReference(taskServiceApi)
    .WithReference(userServiceApi);




builder.Build().Run();
