namespace Nivaes.App.Desktop.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Sample;

    public sealed class AppDesktopSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
