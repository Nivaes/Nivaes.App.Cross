using Android.Content;
using Android.Views;

namespace Nivaes.App.Cross.Droid;

public class MvxAndroidBindingContext
    : CrossBindingContext, IMvxAndroidBindingContext
{
    // Don't remove this or stuff breaks for some reason 🤷‍
    // ReSharper disable once NotAccessedField.Local
#pragma warning disable S4487
    private readonly WeakReference<Context> _context;
#pragma warning restore S4487

    public MvxAndroidBindingContext(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder, object? source = null)
        : base(source)
    {
        _context = new WeakReference<Context>(context);
        LayoutInflaterHolder = layoutInflaterHolder;
    }

    public IMvxLayoutInflaterHolder LayoutInflaterHolder { get; set; }

    public virtual View? BindingInflate(int resourceId, ViewGroup viewGroup)
    {
        return BindingInflate(resourceId, viewGroup, true);
    }

    public virtual View? BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToParent)
    {
        var view = CommonInflate(
            resourceId,
            viewGroup,
            attachToParent);
        return view;
    }

    protected virtual View? CommonInflate(int resourceId, ViewGroup viewGroup, bool attachToParent)
    {
        using (new CrossBindingContextStackRegistration<IMvxAndroidBindingContext>(this))
        {
            var layoutInflater = LayoutInflaterHolder.LayoutInflater;
            {
                // This is most likely a MvxLayoutInflater but it doesn't have to be.
                // It handles setting the bindings and interacts with this instance of
                // MvxAndroidBindingContext through the use of MvxAndroidBindingContextHelpers.Current().
                return layoutInflater.Inflate(resourceId, viewGroup, attachToParent);
            }
        }
    }
}
