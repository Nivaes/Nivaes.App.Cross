using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    internal interface ICrossAndroidContext : ICrossContext
    {
        Context? Context { get; }
    }
}
