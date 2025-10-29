namespace Nivaes.App.Cross
{
    using System.ComponentModel;

    public class CrossNotifyTask 
        : INotifyPropertyChanged
    {
        private Action<Exception>? _onException;

        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        private CrossNotifyTask(Task task, Action<Exception>? onException)
        {
            Task = task;
            _onException = onException;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await Task.Yield();
                await task;
            }
            catch (Exception e)
            {
                _onException?.Invoke(e);
            }
            finally
            {
                NotifyProperties(task);
            }
        }

        private void NotifyProperties(Task task)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            if (task.IsCanceled)
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Exception)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(InnerException)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(ErrorMessage)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsSuccessfullyCompleted)));
            }
            propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsCompleted)));
            propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsNotCompleted)));
        }

        public Task Task { get; private set; }

        public Task TaskCompleted { get; private set; }

        public TaskStatus Status { get { return Task.Status; } }

        public bool IsCompleted { get { return Task.IsCompleted; } }

        public bool IsNotCompleted { get { return !Task.IsCompleted; } }

        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        public bool IsCanceled { get { return Task.IsCanceled; } }

        public bool IsFaulted { get { return Task.IsFaulted; } }

        public AggregateException? Exception { get { return Task.Exception; } }

        public Exception? InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }

        public string? ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static CrossNotifyTask Create(Task task, Action<Exception>? onException = null)
        {
            return new CrossNotifyTask(task, onException);
        }

        public static CrossNotifyTask<TResult> Create<TResult>(Task<TResult> task, TResult? defaultResult = default(TResult), Action<Exception>? onException = null)
        {
            return new CrossNotifyTask<TResult>(task, defaultResult, onException);
        }

        public static CrossNotifyTask Create(Func<Task> asyncAction, Action<Exception>? onException = null)
        {
            return Create(asyncAction(), onException);
        }

        public static CrossNotifyTask<TResult> Create<TResult>(Func<Task<TResult>> asyncAction,
            TResult? defaultResult = default(TResult), Action<Exception>? onException = null)
        {
            return Create(asyncAction(), defaultResult, onException);
        }
    }

    public sealed class CrossNotifyTask<TResult> : INotifyPropertyChanged
    {
        private readonly TResult? _defaultResult;

        private Action<Exception>? _onException;

        internal CrossNotifyTask(Task<TResult> task, TResult? defaultResult, Action<Exception>? onException)
        {
            _defaultResult = defaultResult;
            Task = task;
            _onException = onException;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await System.Threading.Tasks.Task.Yield();
                await task;
            }
            catch (Exception e)
            {
                _onException?.Invoke(e);
            }
            finally
            {
                NotifyProperties(task);
            }
        }

        private void NotifyProperties(Task task)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            if (task.IsCanceled)
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Exception)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(InnerException)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(ErrorMessage)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Result)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsSuccessfullyCompleted)));
            }
            propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsCompleted)));
            propertyChanged(this, CrossPropertyChangedEventArgsCache.Instance.Get(nameof(IsNotCompleted)));
        }

        public Task<TResult> Task { get; private set; }

        public Task TaskCompleted { get; private set; }

        public TResult? Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : _defaultResult; } }

        public TaskStatus Status { get { return Task.Status; } }

        public bool IsCompleted { get { return Task.IsCompleted; } }

        public bool IsNotCompleted { get { return !Task.IsCompleted; } }

        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        public bool IsCanceled { get { return Task.IsCanceled; } }

        public bool IsFaulted { get { return Task.IsFaulted; } }

        public AggregateException? Exception { get { return Task.Exception; } }

        public Exception? InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }

        public string? ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
