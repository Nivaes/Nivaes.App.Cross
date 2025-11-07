namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross.Components.Converters;

    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public class CrossFullBinding
        : CrossBinding, ICrossUpdateableBinding
    {
        private ICrossSourceStepFactory SourceStepFactory => CrossBindingSingletonCache.Instance.SourceStepFactory;

        private ICrossTargetBindingFactory TargetBindingFactory => CrossBindingSingletonCache.Instance.TargetBindingFactory;

        private readonly CrossBindingDescription _bindingDescription;
        private ICrossSourceStep _sourceStep;
        private ICrossTargetBinding _targetBinding;
        private readonly object _targetLocker = new object();

        private object _dataContext;
        private EventHandler _sourceBindingOnChanged;
        private EventHandler<CrossTargetChangedEventArgs> _targetBindingOnValueChanged;

        private object _defaultTargetValue;
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private ICrossMainThreadAsyncDispatcher dispatcher => CrossBindingSingletonCache.Instance.MainThreadDispatcher;

        public object DataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                if (_dataContext == value)
                    return;
                _dataContext = value;

                if (_sourceStep != null)
                    _sourceStep.DataContext = value;

                UpdateTargetOnBind();
            }
        }

        public CrossFullBinding(CrossBindingRequest bindingRequest)
        {
            _bindingDescription = bindingRequest.Description;
            CreateTargetBinding(bindingRequest.Target);
            CreateSourceBinding(bindingRequest.Source);
        }

        protected virtual void ClearSourceBinding()
        {
            if (_sourceStep != null)
            {
                if (_sourceBindingOnChanged != null)
                {
                    _sourceStep.Changed -= _sourceBindingOnChanged;
                    _sourceBindingOnChanged = null;
                }

                _sourceStep.Dispose();
                _sourceStep = null;
            }
        }

        private void CreateSourceBinding(object source)
        {
            // NOTE: We do not call the setter for DataContext here because we are
            // setting up the sourceStep.
            // If that method is updated we will need to make sure that this method
            // does the right thing.
            _dataContext = source;
            _sourceStep = SourceStepFactory.Create(_bindingDescription.Source);
            _sourceStep.TargetType = _targetBinding.TargetValueType;
            _sourceStep.DataContext = source;

            if (NeedToObserveSourceChanges)
            {
                _sourceBindingOnChanged = (sender, args) =>
                {
                    //Capture the cancel token first
                    var cancel = _cancelSource.Token;
                    //GetValue can now be executed in a worker thread. Is it the responsibility of the caller to switch threads, or ours ?
                    //As the source is the viewmodel, i suppose it is the responsibility of the caller.
                    var value = _sourceStep.GetValue();
                    UpdateTargetFromSource(value, cancel);
                };
                _sourceStep.Changed += _sourceBindingOnChanged;
            }

            UpdateTargetOnBind();
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && _sourceStep != null)
            {
                _cancelSource.Cancel();
                _cancelSource = new CancellationTokenSource();
                var cancel = _cancelSource.Token;

                try
                {
                    var currentValue = _sourceStep.GetValue();
                    UpdateTargetFromSource(currentValue, cancel);
                }
                catch (Exception exception)
                {
                    CrossBindingLog.Instance?.LogTrace(exception, "Exception masked in UpdateTargetOnBind");
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            lock (_targetLocker)
            {
                if (_targetBinding != null)
                {
                    if (_targetBindingOnValueChanged != null)
                    {
                        _targetBinding.ValueChanged -= _targetBindingOnValueChanged;
                        _targetBindingOnValueChanged = null;
                    }

                    _targetBinding.Dispose();
                    _targetBinding = null;
                }
            }
        }

        private void CreateTargetBinding(object target)
        {
            _targetBinding = TargetBindingFactory.CreateBinding(target, _bindingDescription.TargetName);

            if (_targetBinding == null)
            {
                CrossBindingLog.Instance?.LogWarning("Failed to create target binding for {BindingDescription}", _bindingDescription.ToString());
                _targetBinding = new CrossNullTargetBinding();
            }

            if (NeedToObserveTargetChanges)
            {
                _targetBinding.SubscribeToEvents();
                _targetBindingOnValueChanged = (sender, args) => UpdateSourceFromTarget(args.Value);
                _targetBinding.ValueChanged += _targetBindingOnValueChanged;
            }

            _defaultTargetValue = _targetBinding.TargetValueType.CreateDefault();
        }

        private async void UpdateTargetFromSource(object value, CancellationToken cancel)
        {
            if (value == CrossBindingConstant.DoNothing || cancel.IsCancellationRequested)
                return;

            if (value == CrossBindingConstant.UnsetValue)
                value = _defaultTargetValue;

            await dispatcher.ExecuteOnMainThreadAsync(() =>
            {
                if (cancel.IsCancellationRequested)
                    return;

                try
                {
                    lock (_targetLocker)
                    {
                        _targetBinding?.SetValue(value);
                    }
                }
                catch (Exception exception)
                {
                    CrossBindingLog.Instance?.LogError(
                        exception,
                        "Problem seen during binding execution for {BindingDescription}",
                        _bindingDescription.ToString());
                }
            });
        }

        private void UpdateSourceFromTarget(object value)
        {
            if (value == CrossBindingConstant.DoNothing)
                return;

            if (value == CrossBindingConstant.UnsetValue)
                return;

            try
            {
                _sourceStep.SetValue(value);
            }
            catch (Exception exception)
            {
                CrossBindingLog.Instance?.LogError(
                    exception,
                    "Problem seen during binding execution for {BindingDescription}",
                    _bindingDescription.ToString());
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

        protected CrossBindingMode ActualBindingMode
        {
            get
            {
                var mode = _bindingDescription.Mode;
                if (mode == CrossBindingMode.Default && _targetBinding != null)
                    mode = _targetBinding.DefaultMode;
                return mode;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearTargetBinding();
                ClearSourceBinding();
            }
        }
    }
}
