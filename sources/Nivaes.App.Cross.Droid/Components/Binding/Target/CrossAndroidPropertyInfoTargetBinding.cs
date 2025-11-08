namespace Nivaes.App.Cross.Droid
{
    using System.Reflection;

    [Obsolete("No compatible con AoT")]
    public abstract class CrossAndroidPropertyInfoTargetBinding(
            object target, PropertyInfo targetPropertyInfo)
        : CrossPropertyInfoTargetBinding(target, targetPropertyInfo)
    {
        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
        {
            return CrossAndroidTargetBinding.TargetIsInvalid(target);
        }
    }

    [Obsolete("No compatible con AoT")]
    public abstract class MvxAndroidPropertyInfoTargetBinding<TView>(
            object target, PropertyInfo targetPropertyInfo)
        : CrossPropertyInfoTargetBinding<TView>(target, targetPropertyInfo)
        where TView : class
    {
        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
        {
            return CrossAndroidTargetBinding.TargetIsInvalid(target);
        }
    }
}