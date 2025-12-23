namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;

    public class CrossBindingContext 
        : ICrossBindingContext, IDisposable
    {
        public record TargetAndBinding(object Target, ICrossUpdateableBinding Binding);

        private readonly List<Action> _delayedActions = new();

        private readonly List<TargetAndBinding> _directBindings = new();

        private readonly List<KeyValuePair<object, IList<TargetAndBinding>>> _viewBindings = new();

        private object _dataContext;

        public CrossBindingContext()
            : this((object)null)
        {
        }

        public CrossBindingContext(object dataContext)
        {
            _dataContext = dataContext;
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext(IDictionary<object, string> firstBindings)
        {
            Init(null, firstBindings);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext(object dataContext, IDictionary<object, string> firstBindings)
        {
            Init(dataContext, firstBindings);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext(IDictionary<object, IEnumerable<CrossBindingDescription>> firstBindings)
        {
            Init(null, firstBindings);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext(object dataContext, IDictionary<object, IEnumerable<CrossBindingDescription>> firstBindings)
        {
            Init(dataContext, firstBindings);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext Init(object dataContext, IDictionary<object, IEnumerable<CrossBindingDescription>> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                AddDelayedAction(kvp);
            }
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public CrossBindingContext Init(object dataContext, IDictionary<object, string> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                AddDelayedAction(kvp);
            }
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<CrossBindingDescription> firstBindingValue)
        {
            AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue)
        {
            AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        private void AddDelayedAction(object key, string value)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, key, value);
                foreach (var b in bindings)
                    RegisterBinding(key, b);
            });
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        private void AddDelayedAction(object key, IEnumerable<CrossBindingDescription> value)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, key, value);
                foreach (var b in bindings)
                    RegisterBinding(key, b);
            });
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        private void AddDelayedAction(KeyValuePair<object, string> kvp)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    RegisterBinding(kvp.Key, b);
            });
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        private void AddDelayedAction(KeyValuePair<object, IEnumerable<CrossBindingDescription>> kvp)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    RegisterBinding(kvp.Key, b);
            });
        }

        ~CrossBindingContext()
        {
            Dispose(false);
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
                ClearAllBindings();
            }
        }

        private ICrossBinder _binder;

        protected ICrossBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.IoCProvider.Resolve<ICrossBinder>();
                return _binder;
            }
        }

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
                OnDataContextChange();
                DataContextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataContextChanged;

        protected virtual void OnDataContextChange()
        {
            // update existing bindings
            foreach (var binding in _viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.Binding.DataContext = _dataContext;
                }
            }

            foreach (var binding in _directBindings)
            {
                binding.Binding.DataContext = _dataContext;
            }

            // add new bindings
            if (_delayedActions.Count == 0)
            {
                return;
            }

            foreach (var action in _delayedActions)
            {
                action();
            }
            _delayedActions.Clear();
        }

        public virtual void DelayBind(Action action)
        {
            _delayedActions.Add(action);
        }

        public virtual void RegisterBinding(object target, ICrossUpdateableBinding binding)
        {
            _directBindings.Add(new TargetAndBinding(target, binding));
        }

        public virtual void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, ICrossUpdateableBinding>> bindings)
        {
            _viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, bindings.Select(b => new TargetAndBinding(b.Key, b.Value)).ToList()));
        }

        public virtual void RegisterBindingWithClearKey(object clearKey, object target, ICrossUpdateableBinding binding)
        {
            var list = new List<TargetAndBinding>() { new TargetAndBinding(target, binding) };
            _viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, list));
        }

        public virtual void ClearBindings(object clearKey)
        {
            if (clearKey == null)
                return;

            for (var i = _viewBindings.Count - 1; i >= 0; i--)
            {
                var candidate = _viewBindings[i];
                if (candidate.Key.Equals(clearKey))
                {
                    foreach (var binding in candidate.Value)
                    {
                        binding.Binding.Dispose();
                    }
                    _viewBindings.RemoveAt(i);
                }
            }
        }

        public virtual void ClearAllBindings()
        {
            ClearAllViewBindings();
            ClearAllDirectBindings();
            ClearAllDelayedBindings();
        }

        protected virtual void ClearAllDelayedBindings()
        {
            _delayedActions.Clear();
        }

        protected virtual void ClearAllDirectBindings()
        {
            foreach (var binding in _directBindings)
            {
                binding.Binding.Dispose();
            }
            _directBindings.Clear();
        }

        protected virtual void ClearAllViewBindings()
        {
            foreach (var kvp in _viewBindings)
            {
                foreach (var binding in kvp.Value)
                {
                    binding.Binding.Dispose();
                }
            }
            _viewBindings.Clear();
        }
    }
}
