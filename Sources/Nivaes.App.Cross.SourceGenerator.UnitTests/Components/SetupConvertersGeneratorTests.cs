//using System;
//using System.Collections.Generic;
//using System.Reflection.Emit;
//using System.Text;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Nivaes.App.Cross.SourceGenerator;

//namespace Nivaes.App.Cross.SourceGenerator.UnitTest
//{
//    public class SetupConvertersGeneratorTests
//    {
//        [Fact]
//        public void ResolveSetupConvertersGenerator()
//        { }

//        [Fact]
//        public void Generates_Source()
//        {
//            // Arrange
//            var source = """
//        namespace Test;

//        public partial class Person
//        {
//        }
//        """;

//            var syntaxTree = CSharpSyntaxTree.ParseText(source);

//            var compilation = CSharpCompilation.Create(
//                "TestAssembly",
//                new[] { syntaxTree },
//                new[]
//                {
//                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
//                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
//                },
//                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

//            IIncrementalGenerator generator = new SetupConvertersGenerator();

//            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

//            // Act
//            driver = driver.RunGenerators(compilation);

//            var result = driver.GetRunResult();

//            // Assert
//            Assert.Single(result.Results);

//            var generatedSources = result.Results[0].GeneratedSources;

//            Assert.Single(generatedSources);

//            var generatedCode = generatedSources[0].SourceText.ToString();

//            Assert.Contains("partial class Person", generatedCode);
//        }
//    }
//}
