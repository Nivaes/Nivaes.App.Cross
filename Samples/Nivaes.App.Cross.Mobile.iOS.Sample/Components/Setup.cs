namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Platforms.Ios.Core;
    using Nivaes.App.Mobile.Sample;

    public sealed class Setup :
        MvxIosSetup<AppMobileSampleApplication<AppMobileSampleAppStart>>
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
        }
    }
}
