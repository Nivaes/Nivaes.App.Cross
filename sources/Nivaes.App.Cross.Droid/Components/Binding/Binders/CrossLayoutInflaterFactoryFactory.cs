namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossLayoutInflaterFactoryFactory
        : ICrossLayoutInflaterHolderFactoryFactory
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        public ICrossLayoutInflaterHolderFactory Create(object source)
        {
            return new CrossBindingLayoutInflaterFactory(source);
        }
    }
}
