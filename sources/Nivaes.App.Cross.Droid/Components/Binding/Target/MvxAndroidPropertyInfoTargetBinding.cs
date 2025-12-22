namespace Nivaes.App.Cross.Droid
{
    using System.Reflection;
    using Nivaes.App.Cross;

    public abstract class MvxAndroidPropertyInfoTargetBinding(
            object target, PropertyInfo targetPropertyInfo)
        : CrossPropertyInfoTargetBinding(target, targetPropertyInfo)
    {
        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }

    public abstract class MvxAndroidPropertyInfoTargetBinding<TView>(
            object target, PropertyInfo targetPropertyInfo)
        : MvxPropertyInfoTargetBinding<TView>(target, targetPropertyInfo)
        where TView : class
    {
        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
        {
            return MvxAndroidTargetBinding.TargetIsInvalid(target);
        }
    }
}