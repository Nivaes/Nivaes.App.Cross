namespace Nivaes.App.Cross
{
    using MvvmCross.IoC;

    [Obsolete("", true)]
    public interface IMvxPlugin
    {
        void Load(IMvxIoCProvider provider);
    }
}
