namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;

    public static class CrossBindingModeExtensions
    {
        public static CrossBindingMode IfDefault(this CrossBindingMode bindingMode, CrossBindingMode modeIfDefault)
        {
            if (bindingMode == CrossBindingMode.Default)
                return modeIfDefault;
            return bindingMode;
        }

        public static bool RequireSourceObservation(this CrossBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case CrossBindingMode.Default:
                    CrossBindingLogger.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case CrossBindingMode.OneWay:
                case CrossBindingMode.TwoWay:
                    return true;

                case CrossBindingMode.OneTime:
                case CrossBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new AppException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequiresTargetObservation(this CrossBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case CrossBindingMode.Default:
                    CrossBindingLogger.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case CrossBindingMode.OneWay:
                case CrossBindingMode.OneTime:
                    return false;

                case CrossBindingMode.TwoWay:
                case CrossBindingMode.OneWayToSource:
                    return true;

                default:
                    throw new AppException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequireTargetUpdateOnFirstBind(this CrossBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case CrossBindingMode.Default:
                    CrossBindingLogger.Instance?.LogWarning("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case CrossBindingMode.OneWay:
                case CrossBindingMode.OneTime:
                case CrossBindingMode.TwoWay:
                    return true;

                case CrossBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new AppException("Unexpected ActualBindingMode");
            }
        }
    }
}
