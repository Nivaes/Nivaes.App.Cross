namespace Nivaes.App.Cross.Sample.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Views;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.Droid.Presenters;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using Nivaes.IoC;

    [Application]
    public  class SampleDroidApplication : DroidApplication
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

            var viewsManager = new ViewsManager(new[] { 
                ViewsManager.New<RootViewModel, RootView>(),
                ViewsManager.New<NewWindowViewModel, NewWindowView>(),
                ViewsManager.New<FormViewModel, FormView>(), 
                ViewsManager.New<SubFormViewModel, SubFormView>(),
                ViewsManager.New<SubSubFormViewModel, SubSubFormView>()
            });
            Singleton<ViewsManager>.Add(viewsManager);
            
            var viewPresentationsManager = new ViewPresentationsManager(new[] { 
                ViewPresentationsManager.New<RootView, ActivityViewPresentation>(),
                ViewPresentationsManager.New<NewWindowView, ActivityViewPresentation>(),
                ViewPresentationsManager.New<FormView, ActivityViewPresentation>(),
                ViewPresentationsManager.New<SubFormView, ActivityViewPresentation>(),
                ViewPresentationsManager.New<SubSubFormView, ActivityViewPresentation>()
            });
            Singleton<ViewPresentationsManager>.Add(viewPresentationsManager);
        }
    }
}
