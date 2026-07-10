using Android.Content;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Droid;

public sealed class MvxBindingLayoutInflaterFactory
    : IMvxLayoutInflaterHolderFactory
{
    private readonly object? _source;

    private IMvxAndroidViewFactory? _androidViewFactory;
    private IMvxAndroidViewBinder _binder;

    public MvxBindingLayoutInflaterFactory(object source)
    {
        _source = source;
        _binder = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewBinderFactory>().Create(_source);
    }

    private IMvxAndroidViewFactory? AndroidViewFactory => _androidViewFactory ??= IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewFactory>();

    public IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings => _binder.CreatedBindings;

    public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
    {
        if (name == "fragment")
        {
            return null;
        }

        View? view = AndroidViewFactory?.CreateView(parent, name, context, attrs);
        return BindCreatedView(view!, context, attrs);
    }

    public View BindCreatedView(View view, Context context, IAttributeSet? attrs)
    {
        if (view != null)
            _binder.BindView(view, context, attrs);

        return view;
    }
}
#nullable restore

