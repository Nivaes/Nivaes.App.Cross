namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;

    public interface ICrossCommandHelper
    {
        event EventHandler? CanExecuteChanged;

        void RaiseCanExecuteChanged(object sender);
    }

    public class CrossStrongCommandHelper
        : ICrossCommandHelper
    {
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged(object sender)
        {
            CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }

    public class CrossWeakCommandHelper
        : ICrossCommandHelper
    {
        private readonly List<WeakReference> _eventHandlers = [];
        private readonly object _syncRoot = new();

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                lock (_syncRoot)
                {
                    _eventHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                lock (_syncRoot)
                {
                    foreach (var thing in _eventHandlers)
                    {
                        var target = thing.Target;
                        if (target != null && (EventHandler)target == value)
                        {
                            _eventHandlers.Remove(thing);
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<EventHandler> SafeCopyEventHandlerList()
        {
            lock (_syncRoot)
            {
                var toReturn = new List<EventHandler>();
                var deadEntries = new List<WeakReference>();

                foreach (var thing in _eventHandlers)
                {
                    if (!thing.IsAlive)
                    {
                        deadEntries.Add(thing);
                        continue;
                    }

                    if (thing.Target is EventHandler eventHandler)
                    {
                        toReturn.Add(eventHandler);
                    }
                }

                foreach (var weakReference in deadEntries)
                {
                    _eventHandlers.Remove(weakReference);
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

    public class CrossCommandBase
        : CrossMainThreadDispatchingObject
    {
        private readonly ICrossCommandHelper _commandHelper;

        protected CrossCommandBase()
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossCommandHelper? commandHelper) == true && commandHelper != null)
            {
                _commandHelper = commandHelper;
            }
            else
            {
                // fallback on MvxWeakCommandHelper if no IoC has been set up
                _commandHelper = new CrossWeakCommandHelper();
            }

            // default to true if no Singleton Cache has been set up
            var alwaysOnUIThread =
                CrossSingletonCache.Instance?.Settings?.AlwaysRaiseInpcOnUserInterfaceThread ?? true;
            ShouldAlwaysRaiseCECOnUserInterfaceThread = alwaysOnUIThread;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => _commandHelper.CanExecuteChanged += value;
            remove => _commandHelper.CanExecuteChanged -= value;
        }

        public bool ShouldAlwaysRaiseCECOnUserInterfaceThread { get; set; }

        public void RaiseCanExecuteChanged()
        {
            if (ShouldAlwaysRaiseCECOnUserInterfaceThread)
            {
                InvokeOnMainThread(() => _commandHelper.RaiseCanExecuteChanged(this));
            }
            else
            {
                _commandHelper.RaiseCanExecuteChanged(this);
            }
        }
    }

    public class CrossCommand
        : CrossCommandBase
        , ICrossCommand
    {
        private readonly Func<bool>? _canExecute;
        private readonly Action _execute;

        public CrossCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute();

        public bool CanExecute()
            => CanExecute(null);

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        public void Execute()
            => Execute(null);
    }

    public class MvxCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>
        : CrossCommandBase
        , ICrossCommand, ICrossCommand<T>
    {
        private readonly Func<T?, bool>? _canExecute;
        private readonly Action<T?> _execute;

        public MvxCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute((T?)typeof(T).MakeSafeValueCore(parameter));

        public bool CanExecute()
            => CanExecute(default);

        public bool CanExecute(T? parameter)
            => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute((T?)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute()
            => Execute(default);

        public void Execute(T? parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute(parameter);
        }
    }
}
