namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public static class CrossBindingModeExtensions
    {
        public static MvxBindingMode IfDefault(this MvxBindingMode bindingMode, MvxBindingMode modeIfDefault)
        {
            if (bindingMode == MvxBindingMode.Default)
                return modeIfDefault;
            return bindingMode;
        }

        public static bool RequireSourceObservation(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxBindingLog.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.TwoWay:
                    return true;

                case MvxBindingMode.OneTime:
                case MvxBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new CrossException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequiresTargetObservation(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxBindingLog.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.OneTime:
                    return false;

                case MvxBindingMode.TwoWay:
                case MvxBindingMode.OneWayToSource:
                    return true;

                default:
                    throw new CrossException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequireTargetUpdateOnFirstBind(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxBindingLog.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.OneTime:
                case MvxBindingMode.TwoWay:
                    return true;

                case MvxBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new CrossException("Unexpected ActualBindingMode");
            }
        }
    }
}
