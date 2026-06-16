namespace MvvmCross.Platforms.WinUi
{
    using MvvmCross.IoC;
    using Nivaes.App.Cross;
    using Windows.ApplicationModel;

    [Obsolete("", true)]
    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (CrossSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                throw new InvalidOperationException();
                //Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }
        }

        private static bool? _isInDesignTime;

        protected static bool IsInDesignTool
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                    _isInDesignTime = DesignMode.DesignModeEnabled;
                return _isInDesignTime.Value;
            }
        }
    }
}
