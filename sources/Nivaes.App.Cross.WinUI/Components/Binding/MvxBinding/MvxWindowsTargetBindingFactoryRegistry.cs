namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Platforms.WinUi.Binding;

    public class MvxWindowsTargetBindingFactoryRegistry 
        : CrossTargetBindingFactoryRegistry
    {
        protected override bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                                out ICrossTargetBinding binding)
        {
            if (TryCreatePropertyDependencyBasedBinding(target, targetName, out binding))
            {
                return true;
            }

            return base.TryCreateReflectionBasedBinding(target, targetName, out binding);
        }

        private static bool TryCreatePropertyDependencyBasedBinding(object target, string targetName,
                                                             out ICrossTargetBinding binding)
        {
            if (target == null)
            {
                binding = null;
                return false;
            }

            if (string.IsNullOrEmpty(targetName))
            {
                CrossBindingLog.Instance?.LogError("Empty binding target passed to MvxWindowsTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            var dependencyProperty = target.GetType().FindDependencyProperty(targetName);
            if (dependencyProperty == null)
            {
                binding = null;
                return false;
            }

            var actualProperty = target.GetType().FindActualProperty(targetName);
            var actualPropertyType = actualProperty?.PropertyType ?? typeof(object);

            binding = new MvxDependencyPropertyTargetBinding(target, targetName, dependencyProperty, actualPropertyType);
            return true;
        }
    }
}
