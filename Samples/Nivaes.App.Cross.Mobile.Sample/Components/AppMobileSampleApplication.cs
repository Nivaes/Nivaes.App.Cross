namespace Nivaes.App.Cross.Mobile.Sample
{
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Sample;

    public sealed class AppMobileSampleApplication<TType>
        : AppSampleApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
