namespace Nivaes.App.Cross.Droid
{
    public sealed class MvxLayoutInflaterFactoryFactory
        : IMvxLayoutInflaterHolderFactoryFactory
    {
        public IMvxLayoutInflaterHolderFactory Create(object? source)
        {
            return new MvxBindingLayoutInflaterFactory(source);
        }
    }
}
