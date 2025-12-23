namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class MvxLayoutInflaterFactoryFactory
        : IMvxLayoutInflaterHolderFactoryFactory
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        public IMvxLayoutInflaterHolderFactory Create(object source)
        {
            return new MvxBindingLayoutInflaterFactory(source);
        }
    }
}
