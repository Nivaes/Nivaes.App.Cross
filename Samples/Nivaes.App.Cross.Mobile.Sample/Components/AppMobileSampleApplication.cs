namespace Nivaes.App.Cross.Mobile.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Sample;

    public sealed class AppMobileSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
