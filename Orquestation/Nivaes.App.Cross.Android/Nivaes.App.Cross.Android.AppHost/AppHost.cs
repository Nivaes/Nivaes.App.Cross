var builder = DistributedApplication.CreateBuilder(args);

var droid = builder.AddProject<Projects.Nivaes_App_Cross_Sample_Droid>("droid");

builder.Build().Run();
