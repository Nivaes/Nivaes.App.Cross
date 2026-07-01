namespace Nivaes.IoC.SourceGenerator
{
    using Microsoft.CodeAnalysis;

    [Generator(LanguageNames.CSharp)]
    public class CrossServiceContainerGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //#if DEBUG
            //            System.Diagnostics.Debugger.Launch();
            //#endif
        }
    }
}
