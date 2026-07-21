using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

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
    private readonly Lock _lock = new();

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            lock (_lock)
            {
                _eventHandlers.Add(new WeakReference(value));
            }
        }
        remove
        {
            lock (_lock)
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
        lock (_lock)
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

public abstract class CrossCommandBase
    : CrossMainThreadDispatchingObject
{
    private readonly ICrossCommandHelper _commandHelper;

    protected CrossCommandBase(ILogger logger)
        : base(logger)
    {
        _commandHelper = new CrossWeakCommandHelper();

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
        : base(CrossLoggerHost.GetLogger<CrossCommand>())
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

public class CrossCommand<T>
    : CrossCommandBase
    , ICrossCommand, ICrossCommand<T>
{
    private readonly Func<T?, bool>? _canExecute;
    private readonly Action<T?> _execute;

    public CrossCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        : base(CrossLoggerHost.GetLogger<CrossCommand<T>>())
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
