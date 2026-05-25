namespace Nivaes.App.Cross.WinUI
{
    using MvvmCross;
    using MvvmCross.IoC;
    using Nivaes.App.Cross;
    using Windows.ApplicationModel;

    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;

            if (!DesignMode.DesignModeEnabled)
                return;

            throw new InvalidOperationException();

            //if (CrossSingleton<IMvxIoCProvider>.Instance == null)
            //{
            //    var iocProvider = MvxIoCProvider.Initialize();
                
            //    Mvx.IoCProvider.RegisterSingleton(iocProvider);
            //}

            //if (!Mvx.IoCProvider.CanResolve<ICrossBindingParser>())
            //{
            //    var builder = new MvxWindowsBindingBuilder(bindingType: MvxWindowsBindingBuilder.BindingType.MvvmCross);
            //    builder.DoRegistration(Mvx.IoCProvider);
            //}
        }
    }
}
