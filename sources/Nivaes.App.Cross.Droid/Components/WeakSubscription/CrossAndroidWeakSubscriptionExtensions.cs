namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class CrossAndroidWeakSubscriptionExtensions
    {
        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource>(TSource source) 
            where TSource : class
        {
            public CrossJavaEventSubscription<TSource> DroidWeakSubscribe(string eventName, EventHandler eventHandler)
            {
                return new CrossJavaEventSubscription<TSource>(source, eventName, eventHandler);
            }

            public CrossAndroidTargetEventSubscription<TSource, TEventArgs> DroidWeakSubscribe<TEventArgs>
                            (string eventName, EventHandler<TEventArgs> eventHandler)
            {
                return new CrossAndroidTargetEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
            }
        }
    }
}
