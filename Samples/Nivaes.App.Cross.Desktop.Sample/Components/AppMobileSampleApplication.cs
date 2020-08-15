namespace Nivaes.App.Desktop.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Sample;

    public sealed class AppMobileSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
