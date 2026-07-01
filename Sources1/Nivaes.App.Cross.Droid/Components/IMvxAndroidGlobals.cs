namespace Nivaes.App.Cross.Droid
{
    using System.Reflection;
    using Android.Content;

    public interface IMvxAndroidGlobals
    {
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}
