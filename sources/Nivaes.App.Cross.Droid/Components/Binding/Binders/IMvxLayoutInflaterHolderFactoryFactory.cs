namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxLayoutInflaterHolderFactoryFactory
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        IMvxLayoutInflaterHolderFactory Create(object? source);
    }
}
