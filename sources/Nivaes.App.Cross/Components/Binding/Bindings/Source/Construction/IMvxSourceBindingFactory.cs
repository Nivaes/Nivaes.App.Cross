namespace MvvmCross.Binding.Bindings.Source.Construction
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using Nivaes.App.Cross;

    public interface IMvxSourceBindingFactory
    {
        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        ICrossSourceBinding CreateBinding(object source, string combinedPropertyName);

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        ICrossSourceBinding CreateBinding(object source, IList<IMvxPropertyToken> tokens);
    }
}
