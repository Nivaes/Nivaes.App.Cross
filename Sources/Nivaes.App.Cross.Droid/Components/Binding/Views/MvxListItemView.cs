using Android.Content;
using Android.Runtime;
using Android.Views;
using Object = Java.Lang.Object;

namespace Nivaes.App.Cross.Droid
{
    [Register("nivaes.cross.ListItemView")]
    public class MvxListItemView : Object, IMvxListItemView,
        ICrossBindingContextOwner, View.IOnAttachStateChangeListener
    {
        private readonly IMvxAndroidBindingContext _bindingContext;
        private View _content;
        private object _cachedDataContext;
        private bool _isAttachedToWindow;

        public MvxListItemView(Context context,
            IMvxLayoutInflaterHolder layoutInflaterHolder, object dataContext,
            ViewGroup parent, int templateId)
        {
            _bindingContext = new MvxAndroidBindingContext(context, layoutInflaterHolder, dataContext);
            TemplateId = templateId;
            Content = _bindingContext.BindingInflate(templateId, parent, false);
        }

        public void OnViewAttachedToWindow(View attachedView)
        {
            _isAttachedToWindow = true;

            if (_cachedDataContext != null && DataContext == null)
                DataContext = _cachedDataContext;
        }

        public void OnViewDetachedFromWindow(View detachedView)
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            _isAttachedToWindow = false;
        }

        public ICrossBindingContext BindingContext
        {
            get => _bindingContext;
            set => throw new NotImplementedException("BindingContext is readonly in the list item");
        }

        public View Content
        {
            get => _content;
            set
            {
                _content = value;
                _content.AddOnAttachStateChangeListener(this);
            }
        }

        public virtual object DataContext
        {
            get => _bindingContext.DataContext;
            set
            {
                if (_isAttachedToWindow)
                {
                    _bindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    _bindingContext.DataContext = null;
                }
            }
        }

        public int TemplateId { get; protected set; }
    }
}
