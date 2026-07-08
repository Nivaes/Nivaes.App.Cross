using Microsoft.CodeAnalysis;

namespace Nivaes.App.Cross.SourceGenerator.UnitTests
{
    public class SetupConvertersGeneratorTests
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
        public async Task ProecesValueConverter()
        {
            var project = await TestProject.Project.ApplyToProgram(@"
                using System.Globalization;
                using Microsoft.Extensions.Logging;

                public sealed class TestValueConverter : CrossValueConverter<string, string>
                {
                    public TestValueConverter(ILogger<TestValueConverter> logger)
                        : base(logger)
                    { }

                    protected override string Convert(string value, Type? targetType, object? parameter, CultureInfo? culture)
                    {
                        return value.ToLower();
                    }
                }");

            var newProject = await project.ApplyGenerator<RegisterConvertersGenerator>();

            var assembly = await newProject.CompileToRealAssembly();
            var containerType1 = assembly.GetType("TestProject.TestValueConverter");
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
