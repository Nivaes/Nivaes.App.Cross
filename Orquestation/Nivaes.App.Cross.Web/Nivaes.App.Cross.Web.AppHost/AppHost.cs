var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Nivaes_App_Cross_Sample_Web>("nivaes-app-cross-sample-web");

builder.Build().Run();
