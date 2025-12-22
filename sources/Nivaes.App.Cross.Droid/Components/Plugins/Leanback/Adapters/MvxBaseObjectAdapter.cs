namespace MvvmCross.DroidX.Leanback.Adapters
{
    using System.Collections;
    using System.Collections.Specialized;
    using Android.Runtime;
    using AndroidX.Leanback.Widget;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;

    public abstract class MvxBaseObjectAdapter
        : ObjectAdapter, IMvxObjectAdapter
    {
        public event EventHandler DataSetChanged;

        private IDisposable _subscription;

        protected IMvxAndroidBindingContext BindingContext { get; }

        private IEnumerable _itemsSource;

        public IEnumerable ItemsSource
        {
            get
            {
                return _itemsSource;
            }
            set
            {
                if (ReferenceEquals(_itemsSource, value))
                {
                    return;
                }

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemsSource = value;

                if (_itemsSource != null)
                {
                    if (!(value is ICollection))
                    {
                        MvxAndroidLog.Instance.Log(LogLevel.Warning,
                            "Using a enumerable is not recommended due to performance issues. Consider using an ICollection (e.g. List) as ItemsSource.");
                    }
                }

                var newObservable = _itemsSource as INotifyCollectionChanged;
                if (newObservable != null)
                {
                    _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
                }

                NotifyAndRaiseDataSetChanged();
            }
        }

        protected MvxBaseObjectAdapter() : this(MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(IMvxAndroidBindingContext bindingContext)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(Presenter presenter)
            : this(presenter, MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(Presenter presenter, IMvxAndroidBindingContext bindingContext)
            : base(presenter)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(PresenterSelector presenterSelector)
            : this(presenterSelector, MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(PresenterSelector presenterSelector, IMvxAndroidBindingContext bindingContext)
            : base(presenterSelector)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            NotifyChanged(eventArgs);
        }

        public virtual void NotifyChanged(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Move:
                        NotifyAndRaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        NotifyAndRaiseDataSetChanged();
                        break;
                }
            }
            catch (Exception exception)
            {
                MvxAndroidLog.Instance?.Log(LogLevel.Warning, exception, "Exception masked during Adapter NotifyChanged");
            }
        }

        protected virtual void RaiseDataSetChanged()
        {
            DataSetChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void NotifyAndRaiseDataSetChanged()
        {
            RaiseDataSetChanged();
            NotifyChanged();
        }
    }
}
