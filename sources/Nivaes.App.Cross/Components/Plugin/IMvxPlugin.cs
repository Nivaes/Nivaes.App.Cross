namespace Nivaes.App.Cross
{
    using MvvmCross.IoC;

    public interface IMvxPlugin
    {
        void Load(IMvxIoCProvider provider);
    }
}
