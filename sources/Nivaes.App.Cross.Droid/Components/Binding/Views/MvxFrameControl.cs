namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Views;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    [Register("mvvmcross.platforms.android.binding.views.MvxFrameControl")]
    public class MvxFrameControl
        : FrameLayout, ICrossBindingContextOwner
    {
        private readonly int _templateId;
        private readonly IMvxAndroidBindingContext _bindingContext;

        public MvxFrameControl(Context context, IAttributeSet attrs)
            : this(MvxAttributeHelpers.ReadTemplateId(context, attrs), context, attrs)
        {
        }

        public MvxFrameControl(int templateId, Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _templateId = templateId;

            if (!(context is IMvxLayoutInflaterHolder))
            {
                throw new CrossException("The owning Context for a MvxFrameControl must implement LayoutInflater");
            }

            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
            this.DelayBind(() =>
                {
                    if (Content == null && _templateId != 0)
                    {
                        var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxFrameControl>>();
                        logger?.Log(LogLevel.Trace, "DataContext is {dataContext}", DataContext?.ToString() ?? "Null");

                        Content = _bindingContext.BindingInflate(_templateId, this);
                    }
                });
        }

        protected MvxFrameControl(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected IMvxAndroidBindingContext AndroidBindingContext => _bindingContext;

        public ICrossBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private object? _cachedDataContext;
        private bool _isAttachedToWindow;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                _cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _isAttachedToWindow = true;
            if (_cachedDataContext != null
                && DataContext == null)
            {
                DataContext = _cachedDataContext;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            base.OnDetachedFromWindow();
            _isAttachedToWindow = false;
        }

        private View _content;

        protected View Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnContentSet();
            }
        }

        protected virtual void OnContentSet()
        {
        }

        [CrossSetToNullAfterBinding]
        public object? DataContext
        {
            get
            {
                return _bindingContext.DataContext;
            }
            set
            {
                if (_isAttachedToWindow)
                {
                    _bindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    if (_bindingContext.DataContext != null)
                    {
                        _bindingContext.DataContext = null;
                    }
                }
            }
        }
    }
}
