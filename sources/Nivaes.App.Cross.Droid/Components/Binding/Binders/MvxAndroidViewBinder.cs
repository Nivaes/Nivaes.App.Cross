using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;


public class MvxAndroidViewBinder
    : IMvxAndroidViewBinder
{
    private readonly List<KeyValuePair<object, ICrossUpdateableBinding>> _viewBindings = new List<KeyValuePair<object, ICrossUpdateableBinding>>();
    private readonly Lazy<IMvxAndroidBindingResource> mvxAndroidBindingResource = new Lazy<IMvxAndroidBindingResource>(() => throw new NotImplementedException() /*Mvx.IoCProvider.GetSingleton<IMvxAndroidBindingResource>()*/);

    private readonly object _source;

    public MvxAndroidViewBinder(object source)
    {
        _source = source;
    }

    private ICrossBinder? _binder;

    protected ICrossBinder? Binder => _binder ?? (_binder = IPlatformApplication.Current!.Services.GetRequiredService<ICrossBinder>());

    public IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings => _viewBindings;

    [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
    public virtual void BindView(View view, Context context, IAttributeSet attrs)
    {
        using (
            var typedArray = context.ObtainStyledAttributes(attrs,
                                                            mvxAndroidBindingResource.Value.BindingStylableGroupId))
        {
            int numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                var attributeId = typedArray.GetIndex(i);

                if (attributeId == mvxAndroidBindingResource.Value.BindingBindId)
                {
                    ApplyBindingsFromAttribute(view, typedArray, attributeId);
                }
                else if (attributeId == mvxAndroidBindingResource.Value.BindingLangId)
                {
                    ApplyLanguageBindingsFromAttribute(view, typedArray, attributeId);
                }
            }
            typedArray.Recycle();
        }
    }

    [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
    private void ApplyBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
    {
        try
        {
            var bindingText = typedArray.GetString(attributeId);
            var newBindings = Binder.Bind(_source, view, bindingText);
            StoreBindings(view, newBindings);
        }
        catch (Exception exception)
        {
            CrossBindingLog.Instance?.LogError(exception, "Exception thrown during the view binding");
        }
    }

    private void StoreBindings(View view, IEnumerable<ICrossUpdateableBinding> newBindings)
    {
        if (newBindings != null)
        {
            _viewBindings.AddRange(newBindings.Select(b => new KeyValuePair<object, ICrossUpdateableBinding>(view, b)));
        }
    }

    [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
    private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
    {
        try
        {
            var bindingText = typedArray.GetString(attributeId);
            var newBindings = Binder.LanguageBind(_source, view, bindingText);
            StoreBindings(view, newBindings);
        }
        catch (Exception exception)
        {
            CrossBindingLog.Instance?.LogError(exception, "Exception thrown during the view language binding");
            throw;
        }
    }
}
