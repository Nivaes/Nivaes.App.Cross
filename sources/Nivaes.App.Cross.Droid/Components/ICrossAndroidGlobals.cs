namespace Nivaes.App.Cross.Droid
{
    using System.Reflection;
    using Android.Content;

    [Obsolete()]
    public interface ICrossAndroidGlobals
    {
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}
