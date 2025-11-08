namespace Nivaes.App.Cross.WinUI3
{
    using MvvmCross.IoC;
    using Windows.ApplicationModel;

    public static class CrossDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            throw new NotImplementedException();
            //if (_checked)
            //    return;

            //_checked = true;

            //if (!DesignMode.DesignModeEnabled)
            //    return;

            //if (CrossSingleton<IMvxIoCProvider>.Instance == null)
            //{
            //    var iocProvider = CrossIoCProvider.Initialize();
            //    Cross.IoCProvider.RegisterSingleton(iocProvider);
            //}

            //if (!Cross.IoCProvider.CanResolve<ICrossBindingParser>())
            //{
            //    var builder = new CrossWindowsBindingBuilder(bindingType: CrossWindowsBindingBuilder.BindingType.MvvmCross);
            //    builder.DoRegistration(Cross.IoCProvider);
            //}
        }
    }
}
