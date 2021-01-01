namespace Nivaes.App.Desktop.Sample
{
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Sample;

    public sealed class AppDesktopSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
