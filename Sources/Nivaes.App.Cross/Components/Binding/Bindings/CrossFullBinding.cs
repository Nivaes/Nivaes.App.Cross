using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components;

namespace Nivaes.App.Cross
{
    public class CrossFullBinding
        : CrossBinding, ICrossUpdateableBinding
    {
        private readonly Lock _lock = new();
        private readonly CrossBindingDescription? _bindingDescription;
        private readonly object? _defaultTargetValue;

        private ICrossSourceStep? _sourceStep;
        private ICrossTargetBinding? _targetBinding;
        private object? _dataContext;
        private CancellationTokenSource? _cancelSource = new();

        public object? DataContext
        {
            get => _dataContext;
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;

                lock (_lock)
                {
                    _sourceStep?.DataContext = value;
                }

                UpdateTargetOnBind();
            }
        }

        public CrossFullBinding(CrossBindingRequest bindingRequest)
        {
            _dataContext = bindingRequest.Source;
            _bindingDescription = bindingRequest.Description;
            var targetBindingFactory = IPlatformApplication.Current!.Services.GetRequiredService<ICrossTargetBindingFactory>();
            _targetBinding = targetBindingFactory.CreateBinding(bindingRequest.Target!, bindingRequest.Description!.TargetName!);

            ObserveTargetChangesIfNeeded();
            _defaultTargetValue = _targetBinding!.TargetValueType.CreateDefault();
            _sourceStep = CreateSourceBinding(bindingRequest);

            UpdateTargetOnBind();
        }

        protected virtual void ClearSourceBinding()
        {
            lock (_lock)
            {
                if (_sourceStep != null)
                {
                    _sourceStep.Changed -= OnSourceBindingChanged;
                    _sourceStep.Dispose();
                }

                _sourceStep = null;
            }
        }

        private ICrossSourceStep CreateSourceBinding(CrossBindingRequest bindingRequest)
        {
            var sourceStep = Singleton<CrossBindingSingletonCache>.Instance.SourceStepFactory.Create(bindingRequest.Description!.Source!);
            sourceStep.TargetType = _targetBinding!.TargetValueType;
            sourceStep.DataContext = bindingRequest.Source;

            if (NeedToObserveSourceChanges)
            {
                sourceStep.Changed += OnSourceBindingChanged;
            }

            return sourceStep;
        }

        private void OnSourceBindingChanged(object? sender, EventArgs e)
        {
            var value = _sourceStep!.GetValue();
            CancellationToken cancel;
            lock (_lock)
            {
                cancel = _cancelSource!.Token;
            }
            UpdateTargetFromSource(value, cancel);
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && _sourceStep != null)
            {
                CancellationToken cancel;
                lock (_lock)
                {
                    _cancelSource!.Cancel();
                    _cancelSource!.Dispose();
                    _cancelSource = new CancellationTokenSource();
                    cancel = _cancelSource.Token;
                }

                try
                {
                    var currentValue = _sourceStep.GetValue();
                    UpdateTargetFromSource(currentValue, cancel);
                }
                catch (Exception ex)
                {
                    CrossBindingLogger.GetLogger<CrossFullBinding>().LogError(ex, "Exception masked in UpdateTargetOnBind");
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            lock (_lock)
            {
                if (_targetBinding != null)
                {
                    _targetBinding.ValueChanged -= UpdateSourceFromTarget;
                    _targetBinding.Dispose();
                    _targetBinding = null;
                }
            }
        }

        private void ObserveTargetChangesIfNeeded()
        {
            if (NeedToObserveTargetChanges && _targetBinding != null)
            {
                _targetBinding.SubscribeToEvents();
                _targetBinding.ValueChanged += UpdateSourceFromTarget;
            }
        }

        private async void UpdateTargetFromSource(object? value, CancellationToken cancel)
        {
            if (value == CrossBindingConstant.DoNothing || cancel.IsCancellationRequested)
                return;

            if (value == CrossBindingConstant.UnsetValue)
            {
                lock (_lock)
                {
                    value = _defaultTargetValue!;
                }
            }

            await Singleton<CrossBindingSingletonCache>.Instance.MainThreadDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                if (cancel.IsCancellationRequested)
                    return;

                try
                {
                    lock (_lock)
                    {
                        _targetBinding?.SetValue(value);
                    }
                }
                catch (Exception exception)
                {
                    CrossBindingLogger.GetLogger<CrossFullBinding>().LogError(
                        exception,
                        "Problem seen during binding execution for {BindingDescription}",
                        _bindingDescription!.ToString());
                }
            });
        }

        private void UpdateSourceFromTarget(object? sender, CrossTargetChangedEventArgs args)
        {
            if (args.Value == CrossBindingConstant.DoNothing)
                return;

            if (args.Value == CrossBindingConstant.UnsetValue)
                return;

            try
            {
                lock (_lock)
                {
                    _sourceStep?.SetValue(args.Value!);
                }
            }
            catch (Exception exception)
            {
                CrossBindingLogger.GetLogger<CrossFullBinding>()?.LogError(
                    exception,
                    "Problem seen during binding execution for {BindingDescription}",
                    _bindingDescription!.ToString());
            }
        }

        protected bool NeedToObserveSourceChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequireSourceObservation();
            }
        }

        protected bool NeedToObserveTargetChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequiresTargetObservation();
            }
        }

        protected bool NeedToUpdateTargetOnBind
        {
            get
            {
                var bindingMode = ActualBindingMode;
                return bindingMode.RequireTargetUpdateOnFirstBind();
            }
        }

        protected internal CrossBindingMode ActualBindingMode
        {
            get
            {
                lock (_lock)
                {
                    var mode = _bindingDescription!.Mode;
                    if (mode == CrossBindingMode.Default && _targetBinding != null)
                        mode = _targetBinding.DefaultMode;
                    return mode;
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearTargetBinding();
                ClearSourceBinding();

                lock (_lock)
                {
                    _cancelSource?.Cancel();
                    _cancelSource?.Dispose();
                    _cancelSource = null;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}
