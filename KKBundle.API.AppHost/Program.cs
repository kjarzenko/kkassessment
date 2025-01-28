var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.KKBundle_API>("kkbundle-api");

builder.Build().Run();
