namespace MvvmCross.Binding.Bindings.Source.Construction
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using Nivaes.App.Cross;

    public interface IMvxSourceBindingFactoryExtension
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        bool TryCreateBinding(
            object source,
            IMvxPropertyToken propertyToken,
            List<IMvxPropertyToken> remainingTokens,
            out ICrossSourceBinding result);
    }
}
