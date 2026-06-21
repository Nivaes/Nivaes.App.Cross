using Android.Runtime;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;   
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public abstract class MvxAndroidTargetBinding
        : CrossConvertingTargetBinding
    {
        protected MvxAndroidTargetBinding(object target)
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
                CrossBindingLogger.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                    javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }

    public abstract class MvxAndroidTargetBinding<TTarget, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue>
        : MvxConvertingTargetBinding<TTarget, TValue>
        where TTarget : class
    {
        protected MvxAndroidTargetBinding(TTarget target)
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
                CrossBindingLogger.Instance?.LogWarning("Weak Target has been GCed by Android {TargetTypeName}",
                    javaTarget.GetType().Name);
                return true;
            }
            return false;
        }
    }
}