namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class CrossAndroidWeakSubscriptionExtensions
    {
        public static CrossJavaEventSubscription<TSource> WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new CrossJavaEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static CrossAndroidTargetEventSubscription<TSource, TEventArgs> WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new CrossAndroidTargetEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}
