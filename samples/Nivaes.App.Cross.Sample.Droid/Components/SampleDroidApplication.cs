namespace Nivaes.App.Cross.Sample.Droid
{
    using System;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.Sample;
    using Nivaes.IoC;

    [Application]
    public  class SampleDroidApplication : DroidCrossApplication
    {
        public SampleDroidApplication(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            // ToDo: Buscar una forma de hacer esto más elegante.
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddDelegate<ICrossApplicationStart>((container) =>
            {
                var navigationService = container.Resolve<ICrossNavigationService>();
                return new SampleApplicationStart(navigationService!);
            });

            container.AddDelegate<ICrossApplication>((container) =>
            {
                var applicationStart = container.Resolve<ICrossApplicationStart>();
                return new SampleApplication(applicationStart!);
            });

            // ToDo: Cargar esto con roslyn
            var viewsManager = new CrossViewsManager(new[] { 
                CrossViewsManager.New<RootViewModel, RootView>(),
                CrossViewsManager.New<NewWindowViewModel, NewWindowView>(),
                CrossViewsManager.New<FormViewModel, FormView>(), 
                CrossViewsManager.New<SubFormViewModel, SubFormView>(),
                CrossViewsManager.New<SubSubFormViewModel, SubSubFormView>()
            });
            Singleton<CrossViewsManager>.Add(viewsManager);
            
            var viewPresentationsManager = new CrossViewPresentationsManager(new[] { 
                CrossViewPresentationsManager.New<RootView, CrossActivityViewPresentation>(),
                CrossViewPresentationsManager.New<NewWindowView, CrossActivityViewPresentation>(),
                CrossViewPresentationsManager.New<FormView, CrossActivityViewPresentation>(),
                CrossViewPresentationsManager.New<SubFormView, CrossActivityViewPresentation>(),
                CrossViewPresentationsManager.New<SubSubFormView, CrossActivityViewPresentation>()
            });
            Singleton<CrossViewPresentationsManager>.Add(viewPresentationsManager);
        }
    }
}
