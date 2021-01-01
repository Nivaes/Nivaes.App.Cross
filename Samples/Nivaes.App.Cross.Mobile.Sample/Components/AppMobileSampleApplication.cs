namespace Nivaes.App.Cross.Mobile.Sample
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;

    public sealed class AppMobileSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
