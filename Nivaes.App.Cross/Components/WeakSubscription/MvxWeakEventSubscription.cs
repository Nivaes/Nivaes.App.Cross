// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;

namespace MvvmCross.WeakSubscription
{
    public class MvxWeakEventSubscription<TSource, TEventArgs> : IDisposable
        where TSource : class
    {
        private readonly WeakReference mTargetReference;
        private readonly WeakReference<TSource> mSourceReference;

        private readonly MethodInfo mEventHandlerMethodInfo;

        private readonly EventInfo mSourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate mOurEventHandler;

        private bool mSubscribed;

        public MvxWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected MvxWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targetEventHandler == null) throw new ArgumentNullException(nameof(targetEventHandler));

            mEventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            mTargetReference = new WeakReference(targetEventHandler.Target);
            mSourceReference = new WeakReference<TSource>(source);
            mSourceEventInfo = sourceEventInfo ?? throw new ArgumentNullException(nameof(sourceEventInfo), "missing source event info in MvxWeakEventSubscription");

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            mOurEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        protected virtual object GetTargetObject()
        {
            return mTargetReference.Target;
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                mEventHandlerMethodInfo.Invoke(target, new[] { sender, e });
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
            if (!mSubscribed)
                return;

            if (mSourceReference.TryGetTarget(out TSource source))
            {
                mSourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { mOurEventHandler });
                mSubscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (mSubscribed)
                throw new MvxException("Should not call _subscribed twice");

            if (mSourceReference.TryGetTarget(out TSource source))
            {
                mSourceEventInfo.GetAddMethod().Invoke(source, new object[] { mOurEventHandler });
                mSubscribed = true;
            }
        }
    }

    public class MvxWeakEventSubscription<TSource> : IDisposable
        where TSource : class
    {
        private readonly WeakReference mTargetReference;
        private readonly WeakReference<TSource> mSourceReference;

        private readonly MethodInfo mEventHandlerMethodInfo;

        private readonly EventInfo mSourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate mOurEventHandler;

        private bool mSubscribed;

        public MvxWeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected MvxWeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler targetEventHandler)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targetEventHandler == null) throw new ArgumentNullException(nameof(targetEventHandler));

            mEventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            mTargetReference = new WeakReference(targetEventHandler.Target);
            mSourceReference = new WeakReference<TSource>(source);
            mSourceEventInfo = sourceEventInfo ?? throw new ArgumentNullException(nameof(sourceEventInfo), "missing source event info in MvxWeakEventSubscription");

            // TODO: need to move this virtual call out of the constructor - need to implement a separate Init() method
            mOurEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        protected virtual object GetTargetObject()
        {
            return mTargetReference.Target;
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, EventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                mEventHandlerMethodInfo.Invoke(target, new[] { sender, e });
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
            if (!mSubscribed)
                return;

            if (mSourceReference.TryGetTarget(out TSource source))
            {
                mSourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { mOurEventHandler });
                mSubscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (mSubscribed)
                throw new MvxException("Should not call _subscribed twice");

            if (mSourceReference.TryGetTarget(out TSource source))
            {
                mSourceEventInfo.GetAddMethod().Invoke(source, new object[] { mOurEventHandler });
                mSubscribed = true;
            }
        }
    }
}
