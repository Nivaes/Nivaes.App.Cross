using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

[RequiresUnreferencedCode("This class creates bindings which use reflection and may not be preserved by trimming.")]
public class MvxBindingLayoutInflaterFactory
    : IMvxLayoutInflaterHolderFactory
{
    private readonly object? _source;

    private IMvxAndroidViewFactory? _androidViewFactory;
    private IMvxAndroidViewBinder? _binder;

    public MvxBindingLayoutInflaterFactory(object? source)
    {
        _source = source;
    }

    protected virtual IMvxAndroidViewFactory? AndroidViewFactory => _androidViewFactory ??= IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewFactory>();

    protected virtual IMvxAndroidViewBinder? Binder => _binder ??= IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewBinderFactory>().Create(_source);

    public virtual IList<KeyValuePair<object, ICrossUpdateableBinding>>? CreatedBindings => Binder?.CreatedBindings;

    public virtual View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
    {
        if (name == "fragment")
        {
            // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
            return null;
        }

        View? view = AndroidViewFactory?.CreateView(parent, name, context, attrs);
        return BindCreatedView(view, context, attrs);
    }

    public virtual View? BindCreatedView(View? view, Context? context, IAttributeSet attrs)
    {
        if (view != null)
            Binder?.BindView(view, context, attrs);
        return view;
    }
}
#nullable restore

