namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossLayoutInflaterHolderFactoryFactory
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        ICrossLayoutInflaterHolderFactory Create(object source);
    }
}
