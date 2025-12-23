namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    public interface ICrossSourceBindingFactory
    {
        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        ICrossSourceBinding CreateBinding(object source, string combinedPropertyName);

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        ICrossSourceBinding CreateBinding(object source, IList<ICrossPropertyToken> tokens);
    }
}
