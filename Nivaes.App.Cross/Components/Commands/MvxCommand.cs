// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Collections.Generic;

    public interface IMvxCommandHelper
    {
        event EventHandler CanExecuteChanged;

        void RaiseCanExecuteChanged(object sender);
    }

    public class MvxStrongCommandHelper
        : IMvxCommandHelper
    {
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged(object sender)
        {
            CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }

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

    public class MvxCommand
        : MvxCommandBase
        , IMvxCommand
    {
        private readonly Func<bool>? mCanExecute;
        private readonly Action mExecute;

        public MvxCommand(Action execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action execute, Func<bool>? canExecute)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => mCanExecute == null || mCanExecute();

        public bool CanExecute()
            => CanExecute(null);

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                mExecute();
            }
        }

        public void Execute()
            => Execute(null);
    }

    public class MvxCommand<T>
        : MvxCommandBase
        , IMvxCommand, IMvxCommand<T>
    {
        private readonly Func<T, bool>? mCanExecute;
        private readonly Action<T> mExecute;

        public MvxCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action<T> execute, Func<T, bool>? canExecute)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => mCanExecute == null || mCanExecute((T)typeof(T).MakeSafeValueCore(parameter));

        public bool CanExecute()
            => CanExecute(null);

        public bool CanExecute(T parameter)
            => mCanExecute == null || mCanExecute(parameter);

        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;

            mExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute()
            => Execute(null);

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter)) return;

            mExecute(parameter);
        }
    }
}
