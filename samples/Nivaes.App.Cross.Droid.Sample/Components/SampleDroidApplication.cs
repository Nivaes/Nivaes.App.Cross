namespace Nivaes.App.Cross.Droid.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
                var navigationService = container.Resolve<INavigationService>();
                return new SampleApplicationStart(navigationService!);
            });

            container.AddDelegate<ICrossApplication>((container) =>
            {
                var applicationStart = container.Resolve<ICrossApplicationStart>();
                return new SampleApplication(applicationStart!);
            });

            var viewsManager = Singleton<ViewsManager>.Instance;
            viewsManager.AddViewModelView<RootViewModel, RootView>();
            //viewsManager.AddViewModelView<NewWindowViewModel, NewWindowView>();

            var viewPresentationsManager = Singleton<ViewPresentationsManager>.Instance;
            //viewPresentationsManager.AddPresentation<RootView, WinUIPageViewPresentation>();
            //viewPresentationsManager.AddPresentation<NewWindowView, WinUINewWindowViewPresentation>();
        }
    }
}
