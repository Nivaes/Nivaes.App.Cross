using System.Buffers;
using System.Globalization;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.SourceGenerator.UnitTests;

public static class TestProject
{
    public const string ProgramCs = @"
using System;
using Nivaes.App.Cross;

namespace TestProject 
{
    // place to replace

    class Program
    {
        static void Main(string[] args)
        {

        }
    }   
}
";

    static TestProject()
    {
        var workspace = new AdhocWorkspace();
        Project = workspace
            .AddProject("TestProject", LanguageNames.CSharp)
            .WithMetadataReferences(GetReferences())
            .AddDocument("Program.cs", ProgramCs).Project;
    }

    public static Project Project { get; }

    private static MetadataReference[] GetReferences()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return new MetadataReference[]
        {
            MetadataReference.CreateFromFile(assemblies.Single(a => a.GetName().Name == "netstandard").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Buffers").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ArrayPool<>).Assembly.Location),
            //MetadataReference.CreateFromFile(typeof(CultureInfo).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ILogger<>).Assembly.Location),
            //MetadataReference.CreateFromFile(typeof(CrossConvertersManagerHelper).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IServiceProvider).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(KeyContainerManager<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICrossValueConverter).Assembly.Location),
            //MetadataReference.CreateFromFile(typeof(StringToLowerValueConverter).Assembly.Location),
        };
    }
}
