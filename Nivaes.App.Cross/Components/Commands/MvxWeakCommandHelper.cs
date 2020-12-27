// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Collections.Generic;

    public class MvxWeakCommandHelper
        : IMvxCommandHelper
    {
        private readonly List<WeakReference> mEventHandlers = new List<WeakReference>();
        private readonly object mSyncRoot = new object();

        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (mSyncRoot)
                {
                    mEventHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                lock (mSyncRoot)
                {
                    foreach (var thing in mEventHandlers)
                    {
                        var target = thing.Target;
                        if (target != null && (EventHandler)target == value)
                        {
                            mEventHandlers.Remove(thing);
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<EventHandler> SafeCopyEventHandlerList()
        {
            lock (mSyncRoot)
            {
                var toReturn = new List<EventHandler>();
                var deadEntries = new List<WeakReference>();

                foreach (var thing in mEventHandlers)
                {
                    if (!thing.IsAlive)
                    {
                        deadEntries.Add(thing);
                        continue;
                    }
                    var eventHandler = (EventHandler)thing.Target;
                    if (eventHandler != null)
                    {
                        toReturn.Add(eventHandler);
                    }
                }

                foreach (var weakReference in deadEntries)
                {
                    mEventHandlers.Remove(weakReference);
                }

                return toReturn;
            }
        }

        public void RaiseCanExecuteChanged(object sender)
        {
            var list = SafeCopyEventHandlerList();
            foreach (var eventHandler in list)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }
    }
}
