namespace Nivaes.App.Cross.Droid
{
    using System.Collections;
    using System.Collections.Specialized;
    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Views;
    using Microsoft.Extensions.Logging;

    [Register("mvvmcross.platforms.android.binding.views.MvxRadioGroup")]
    public class CrossRadioGroup : RadioGroup, ICrossWithChangeAdapter
    {
        public CrossRadioGroup(Context context, IAttributeSet attrs)
            : this(context, attrs, new CrossAdapterWithChangedEvent(context))
        {
        }

        public CrossRadioGroup(Context context, IAttributeSet attrs, ICrossAdapterWithChangedEvent adapter)
            : base(context, attrs)
        {
            var itemTemplateId = CrossAttributeHelpers.ReadListItemTemplateId(context, attrs);
            if (adapter != null)
            {
                Adapter = adapter;
                Adapter.ItemTemplateId = itemTemplateId;
            }

            ChildViewAdded += OnChildViewAdded;
            ChildViewRemoved += OnChildViewRemoved;
        }

        protected CrossRadioGroup(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            this.UpdateDataSetFromChange(sender, eventArgs);
        }

        private void OnChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs args)
        {
            //var li = (args.Child as MvxListItemView);
            var radioButton = args.Child as RadioButton;

            // radio buttons require an id so that they get un-checked correctly
            if (radioButton?.Id == NoId)
            {
                radioButton.Id = GenerateViewId();
            }
        }

        private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
        {
            var boundChild = childViewRemovedEventArgs.Child as ICrossBindingContextOwner;
            boundChild?.ClearAllBindings();
        }

        private ICrossAdapterWithChangedEvent _adapter;

        public ICrossAdapterWithChangedEvent Adapter
        {
            get
            {
                return _adapter;
            }
            protected set
            {
                var existing = _adapter;
                if (existing == value)
                {
                    return;
                }

                if (existing != null)
                {
                    existing.DataSetChanged -= AdapterOnDataSetChanged;
                    if (value != null)
                    {
                        value.ItemsSource = existing.ItemsSource;
                        value.ItemTemplateId = existing.ItemTemplateId;
                    }
                }

                _adapter = value;

                if (_adapter != null)
                {
                    _adapter.DataSetChanged += AdapterOnDataSetChanged;
                }
                else
                {
                    CrossBindingLog.Instance?.LogWarning(
                        "Setting Adapter to null is not recommended - you may lose ItemsSource binding when doing this");
                }

                if (existing != null)
                    existing.ItemsSource = null;
            }
        }

        [CrossSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        private static long _nextGeneratedViewId = 1;

        private static new int GenerateViewId()
        {
            for (; ; )
            {
                int result = (int)Interlocked.Read(ref _nextGeneratedViewId);

                // aapt-generated IDs have the high byte nonzero; clamp to the range under that.
                int newValue = result + 1;
                if (newValue > 0x00FFFFFF)
                {
                    // Roll over to 1, not 0.
                    newValue = 1;
                }

                if (Interlocked.CompareExchange(ref _nextGeneratedViewId, newValue, result) == result)
                {
                    return result;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_adapter != null)
                    _adapter.DataSetChanged -= AdapterOnDataSetChanged;

                ChildViewAdded -= OnChildViewAdded;
                ChildViewRemoved -= OnChildViewRemoved;
            }

            base.Dispose(disposing);
        }
    }
}
