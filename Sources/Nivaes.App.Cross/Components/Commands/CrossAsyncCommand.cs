using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public abstract class CrossAsyncCommandBase
        : CrossCommandBase
    {
        private readonly Lock _lock = new();
        private readonly bool _allowConcurrentExecutions;
        private CancellationTokenSource? _cts;
        private int _concurrentExecutions;

        protected CrossAsyncCommandBase(bool allowConcurrentExecutions = false)
            : base(CrossLoggerHost.GetLogger<CrossAsyncCommandBase>())
        {
            _allowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning => _concurrentExecutions > 0;

        protected CancellationToken CancelToken => _cts?.Token ?? CancellationToken.None;

        protected abstract bool CanExecuteImpl(object? parameter);

        protected abstract Task ExecuteAsyncImpl(object? parameter);

        public void Cancel()
        {
            lock (_lock)
            {
                if (_cts == null)
                {
                    Logger.LogWarning($"{nameof(CrossAsyncCommand)} : Attempt to cancel a task that is not running");
                }
                else
                {
                    _cts.Cancel();
                }
            }
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(object? parameter)
        {
            if (!_allowConcurrentExecutions && IsRunning)
                return false;
            else
                return CanExecuteImpl(parameter);
        }

        public async void Execute(object? parameter)
        {
            try
            {
                await ExecuteAsync(parameter, true).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"{nameof(CrossAsyncCommand)} : exception executing task");
                throw;
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        protected async Task ExecuteAsync(object? parameter, bool hideCanceledException)
        {
            if (CanExecuteImpl(parameter))
            {
                await ExecuteConcurrentAsync(parameter, hideCanceledException).ConfigureAwait(false);
            }
        }

        private async Task ExecuteConcurrentAsync(object? parameter, bool hideCanceledException)
        {
            bool started = false;
            try
            {
                lock (_lock)
                {
                    if (_concurrentExecutions == 0)
                    {
                        InitCancellationTokenSource();
                    }
                    else if (!_allowConcurrentExecutions)
                    {
                        Logger.LogInformation($"{nameof(CrossAsyncCommand)}: execute ignored, already running");
                        return;
                    }
                    _concurrentExecutions++;
                    started = true;
                }

                if (!_allowConcurrentExecutions)
                {
                    RaiseCanExecuteChanged();
                }
                if (!CancelToken.IsCancellationRequested)
                {
                    try
                    {
                        // With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
                        // This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
                        await ExecuteAsyncImpl(parameter).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e)
                    {
                        Logger.LogTrace($"{nameof(CrossAsyncCommand)}: OperationCanceledException");
                        //Rethrow if the exception does not come from the current cancellation token
                        if (!hideCanceledException || e.CancellationToken != CancelToken)
                        {
                            throw;
                        }
                    }
                }
            }
            finally
            {
                if (started)
                {
                    lock (_lock)
                    {
                        _concurrentExecutions--;
                        if (_concurrentExecutions == 0)
                        {
                            ClearCancellationTokenSource();
                        }
                    }
                    if (!_allowConcurrentExecutions)
                    {
                        RaiseCanExecuteChanged();
                    }
                }
            }
        }

        private void ClearCancellationTokenSource()
        {
            if (_cts == null)
            {
                Logger.LogError($"{nameof(CrossAsyncCommand)}: Unexpected ClearCancellationTokenSource, no token available!");
            }
            else
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        private void InitCancellationTokenSource()
        {
            if (_cts != null)
            {
                Logger.LogError($"{nameof(CrossAsyncCommand)}: Unexpected InitCancellationTokenSource, a token is already available!");
            }
            _cts = new CancellationTokenSource();
        }
    }

    public class CrossAsyncCommand
        : CrossAsyncCommandBase
        , ICrossAsyncCommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool>? _canExecute;

        public CrossAsyncCommand(Func<Task> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = _ => execute();
            _canExecute = canExecute;
        }

        public CrossAsyncCommand(Func<CancellationToken, Task> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        protected override Task ExecuteAsyncImpl(object? parameter)
        {
            return _execute(CancelToken);
        }

        public static CrossAsyncCommand<T?> CreateCommand<T>(Func<T?, Task> execute, Func<T?, bool>? canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new CrossAsyncCommand<T?>(execute, canExecute, allowConcurrentExecutions);
        }

        public static CrossAsyncCommand<T?> CreateCommand<T>(Func<T?, CancellationToken, Task> execute, Func<T?, bool>? canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new CrossAsyncCommand<T?>(execute, canExecute, allowConcurrentExecutions);
        }

        public Task ExecuteAsync(object? parameter = null)
        {
            return ExecuteAsync(parameter, false);
        }
    }

    public class CrossAsyncCommand<T>
        : CrossAsyncCommandBase, ICrossCommand, ICrossAsyncCommand<T>
    {
        private readonly Func<T?, CancellationToken, Task> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public CrossAsyncCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));

            _execute = (p, _) => execute(p);
            _canExecute = canExecute;
        }

        public CrossAsyncCommand(Func<T?, CancellationToken, Task> execute, Func<T?, bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = execute;
            _canExecute = canExecute;
        }

        public Task ExecuteAsync(T? parameter)
            => ExecuteAsync(parameter, false);

        public void Execute(T? parameter)
            => base.Execute(parameter);

        public bool CanExecute(T? parameter)
            => base.CanExecute(parameter);

        protected override bool CanExecuteImpl(object? parameter)
            => _canExecute == null || _canExecute((T?)typeof(T).MakeSafeValueCore(parameter));

        protected override Task ExecuteAsyncImpl(object? parameter)
            => _execute((T?)typeof(T).MakeSafeValueCore(parameter), CancelToken);
    }
}
