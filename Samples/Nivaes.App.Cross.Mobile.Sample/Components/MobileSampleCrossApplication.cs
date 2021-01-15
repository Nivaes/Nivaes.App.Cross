namespace Nivaes.App.Cross.Mobile.Sample
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;

    public sealed class MobileSampleCrossApplication<TType>
        : SampleCrossApplication<TType>
          where TType : class, IMvxAppStart
    {
    }
}
