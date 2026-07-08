namespace Nivaes.App.Cross.SourceGenerator
{
    using Microsoft.CodeAnalysis;

    [Generator(LanguageNames.CSharp)]
    public class SetupConvertersGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //#if DEBUG
            //            System.Diagnostics.Debugger.Launch();
            //#endif
        }
    }
}
