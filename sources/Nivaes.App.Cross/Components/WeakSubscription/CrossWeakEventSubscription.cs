namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class CrossWeakEventSubscription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource, TEventArgs> 
        : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public CrossWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, GetEventInfo(sourceEventName), targetEventHandler)
        {
        }

        protected CrossWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(sourceEventInfo);

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);
            _sourceEventInfo = sourceEventInfo;

            _ourEventHandler = Init();

            AddEventHandler();
        }

        private static EventInfo GetEventInfo(string sourceEventName)
        {
            var eventInfo = typeof(TSource).GetEvent(sourceEventName);
            if (eventInfo == null)
                throw new ArgumentOutOfRangeException(sourceEventName);

            return eventInfo;
        }

        private Delegate Init()
        {
            return CreateEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        protected virtual object? GetTargetObject()
        {
            return _targetReference.Target;
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object? sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, [sender, e]);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            try
            {
                if (_sourceReference.TryGetTarget(out var source))
                {
                    _sourceEventInfo.GetRemoveMethod()?.Invoke(source, [_ourEventHandler]);
                    _subscribed = false;
                }
            }
            catch (TargetInvocationException tie) when (tie.InnerException is ObjectDisposedException)
            {
                // we don't care if source has already been disposed
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new CrossException("Should not call AddEventHandler twice");

            if (_sourceReference.TryGetTarget(out var source))
            {
                _sourceEventInfo.GetAddMethod()?.Invoke(source, [_ourEventHandler]);
                _subscribed = true;
            }
        }
    }

    public class CrossWeakEventSubscription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TSource> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public CrossWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler targetEventHandler)
            : this(source, GetEventInfo(sourceEventName), targetEventHandler)
        {
        }

        protected CrossWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler targetEventHandler)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(sourceEventInfo);

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);
            _sourceEventInfo = sourceEventInfo;

            _ourEventHandler = Init();

            AddEventHandler();
        }

        public Delegate Init()
        {
            return CreateEventHandler();
        }

        private static EventInfo GetEventInfo(string sourceEventName)
        {
            var eventInfo = typeof(TSource).GetEvent(sourceEventName);
            if (eventInfo == null)
                throw new ArgumentOutOfRangeException(sourceEventName);

            return eventInfo;
        }

        protected virtual object? GetTargetObject()
        {
            return _targetReference.Target;
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object? sender, EventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, [sender, e]);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            try
            {
                if (_sourceReference.TryGetTarget(out var source))
                {
                    _sourceEventInfo.GetRemoveMethod()?.Invoke(source, new object[] { _ourEventHandler });
                    _subscribed = false;
                }
            }
            catch (TargetInvocationException tie) when (tie.InnerException is ObjectDisposedException)
            {
                // we don't care if source has already been disposed
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new CrossException("Should not call AddEventHandler() twice");

            if (_sourceReference.TryGetTarget(out var source))
            {
                _sourceEventInfo.GetAddMethod()?.Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }
}