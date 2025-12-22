using Android.Runtime;

namespace MvvmCross.Platforms.Android.WeakSubscription
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Nivaes.App.Cross;

    /// <summary>
    /// Weak subscription to an event where the target may be an IJavaObject
    /// and could be collected by the Android runtime before being collected by the Mono GC.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    public class MvxAndroidTargetEventSubscription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource, TEventArgs>
        : CrossWeakEventSubscription<TSource, TEventArgs>
        where TSource : class
    {
        public MvxAndroidTargetEventSubscription(TSource source, string sourceEventName, EventHandler<TEventArgs> targetEventHandler)
            : base(source, sourceEventName, targetEventHandler)
        {
        }

        public MvxAndroidTargetEventSubscription(TSource source, EventInfo sourceEventInfo, EventHandler<TEventArgs> targetEventHandler)
            : base(source, sourceEventInfo, targetEventHandler)
        {
        }

        protected override object GetTargetObject()
        {
            // If the object has been GCed by java but NOT mono
            // then it is invalid and should not be manipulated.
            var target = base.GetTargetObject();
            var javaObj = target as IJavaObject;
            if (javaObj != null && javaObj.Handle == IntPtr.Zero)
            {
                return null;
            }
            return target;
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }
    }

    public class MvxJavaEventSubscription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource> : CrossWeakEventSubscription<TSource> where TSource : class
    {
        public MvxJavaEventSubscription(TSource source, string sourceEventName, EventHandler targetEventHandler)
            : base(source, sourceEventName, targetEventHandler)
        {
        }

        public MvxJavaEventSubscription(TSource source, EventInfo sourceEventInfo, EventHandler targetEventHandler)
            : base(source, sourceEventInfo, targetEventHandler)
        {
        }

        protected override object GetTargetObject()
        {
            // If the object has been GCed by java but NOT mono
            // then it is invalid and should not be manipulated.
            var target = base.GetTargetObject();
            var javaObj = target as IJavaObject;
            if (javaObj != null && javaObj.Handle == IntPtr.Zero)
            {
                return null;
            }
            return target;
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }
    }
}
