namespace MvvmCross.Platforms.Mac.Core
{
    using AppKit;
    using MvvmCross.Core;
    using Nivaes.App.Cross;

    public interface IMvxApplicationDelegate 
        : INSApplicationDelegate, ICrossLifetime
    {
    }
}
