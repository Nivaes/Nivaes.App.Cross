namespace Nivaes.App.Desktop.Sample
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;

    public sealed class AppDesktopSampleApplication<TType>
        : SampleCrossApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
