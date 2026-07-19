using Microsoft.CodeAnalysis;

namespace Nivaes.App.Cross.SourceGenerator.UnitTests
{
    public class RegisterPresenterActionsGeneratorTests
    {
        [Fact]
        public async Task CompilesWithoutErrors()
        {
            var project = TestProject.Project;

            var newProject = await project.ApplyGenerator<RegisterPresenterActionsGenerator>();

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
                using System.Threading.Tasks;
                using Microsoft.Extensions.Logging;
                using Nivaes.App.Cross;

                public sealed class TestPresentationAttribute : BasePresentationAttribute
                {
                }

                public sealed class TestPressenterAction : PressenterAction<TestPresentationAttribute>
                {
                    public TestPressenterAction(ILogger<TestPressenterAction> logger)
                        :base(logger)
                    { }

                    protected override ValueTask<bool> ShowAction(Type viewType, TestPresentationAttribute attribute, ViewModelRequest request)
                    {
                        throw new NotImplementedException();
                    }
                    protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, TestPresentationAttribute attribute)
                    {
                        throw new NotImplementedException();
                    }

                    protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
                    {
                        throw new NotImplementedException();
                    }
                }");

            var newProject = await project.ApplyGenerator<RegisterPresenterActionsGenerator>();

            var assembly = await newProject.CompileToRealAssembly();
            var containerType1 = assembly.GetType("TestProject.TestPresentationAttribute");
            containerType1.ShouldNotBeNull();
            var containerType2 = assembly.GetType("TestProject.TestPressenterAction");
            containerType2.ShouldNotBeNull();

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
