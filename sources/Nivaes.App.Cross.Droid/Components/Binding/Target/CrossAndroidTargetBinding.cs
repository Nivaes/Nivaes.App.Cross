namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Runtime;
    using Microsoft.Extensions.Logging;

    public abstract class CrossAndroidTargetBinding
        : CrossConvertingTargetBinding
    {
        protected CrossAndroidTargetBinding(object target)
            : base(target)
        {
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(object target)
        {
            if (target is IJavaObject javaTarget && javaTarget.Handle == IntPtr.Zero)
            {
                CrossBindingLog.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                    javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }

    public abstract class CrossAndroidTargetBinding<TTarget, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue>
        : CrossConvertingTargetBinding<TTarget, TValue>
        where TTarget : class
    {
        protected CrossAndroidTargetBinding(TTarget target)
            : base(target)
        {
        }

        protected override bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue? value)
        {
            return TargetIsInvalid(target);
        }

        public static bool TargetIsInvalid(TTarget target)
        {
            if (target is IJavaObject javaTarget && javaTarget.Handle == IntPtr.Zero)
            {
                CrossBindingLog.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                    javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }
}