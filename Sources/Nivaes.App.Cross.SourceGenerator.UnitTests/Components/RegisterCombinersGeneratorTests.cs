using Microsoft.CodeAnalysis;

namespace Nivaes.App.Cross.SourceGenerator.UnitTests
{
    public class RegisterCombinersGeneratorTests
    {
        [Fact]
        public async Task CompilesWithoutErrors()
        {
            var project = TestProject.Project;

            var newProject = await project.ApplyGenerator<RegisterConvertersGenerator>();

            var compilation = await newProject.GetCompilationAsync();
            compilation.ShouldNotBeNull();
            var errors = compilation.GetDiagnostics()
                .Where(o => o.Severity == DiagnosticSeverity.Error)
                .ToArray();

            //Assert.False(errors.Any(), errors.Select(o => o.GetMessage()));//.JoinWithNewLine()); 
            errors.ShouldBeEmpty(errors.ToString());
        }

        [Fact]
        public async Task ProccesValueCombiner()
        {
            var project = await TestProject.Project.ApplyToProgram(@"
                using System.Globalization;
                using Microsoft.Extensions.Logging;

                public sealed class TestValueCombiner : CrossValueCombiner
                {
                    public TestValueCombiner(ILogger<TestValueCombiner> logger)
                        : base(logger)
                    { }
                }");

            var newProject = await project.ApplyGenerator<RegisterConvertersGenerator>();

            var assembly = await newProject.CompileToRealAssembly();
            var containerType1 = assembly.GetType("TestProject.TestValueCombiner");
            containerType1.ShouldNotBeNull();

            var compilation = await newProject.GetCompilationAsync();
            compilation.ShouldNotBeNull();
            var errors = compilation.GetDiagnostics()
                .Where(o => o.Severity == DiagnosticSeverity.Error)
                .ToArray();

            //Assert.False(errors.Any(), errors.Select(o => o.GetMessage()));//.JoinWithNewLine()); 
            errors.ShouldBeEmpty(errors.ToString());
        }

        [Fact]
        public async Task ReadAtribute()
        {
            var project = await TestProject.Project.ApplyToProgram(@"
                using System.Globalization;
                using Microsoft.Extensions.Logging;
                
                [CrossValueCombiner(Name=""Test"")]
                public sealed class TestValueCombiner : CrossValueCombiner
                {
                    public TestValueCombiner(ILogger<TestValueCombiner> logger)
                        : base(logger)
                    { }
                }");

            var newProject = await project.ApplyGenerator<RegisterConvertersGenerator>();

            var assembly = await newProject.CompileToRealAssembly();
            var containerType1 = assembly.GetType("TestProject.TestValueCombiner");
            containerType1.ShouldNotBeNull();

            var compilation = await newProject.GetCompilationAsync();
            compilation.ShouldNotBeNull();
            var errors = compilation.GetDiagnostics()
                .Where(o => o.Severity == DiagnosticSeverity.Error)
                .ToArray();

            //Assert.False(errors.Any(), errors.Select(o => o.GetMessage()));//.JoinWithNewLine()); 
            errors.ShouldBeEmpty(errors.ToString());
        }

    }
}
