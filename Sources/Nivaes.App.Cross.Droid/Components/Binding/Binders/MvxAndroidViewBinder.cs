using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public sealed class MvxAndroidViewBinder
    : IMvxAndroidViewBinder
{
    private readonly List<KeyValuePair<object, ICrossUpdateableBinding>> _viewBindings = new List<KeyValuePair<object, ICrossUpdateableBinding>>();
    private readonly Lazy<IMvxAndroidBindingResource> mvxAndroidBindingResource = new Lazy<IMvxAndroidBindingResource>(() =>
          IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxAndroidBindingResource>());

    private readonly object? _source;

    public MvxAndroidViewBinder(object? source)
    {
        _source = source;
    }

    private ICrossBinder? _binder;

    private ICrossBinder? Binder => _binder ?? (_binder = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBinder>());

    public IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings => _viewBindings;
  
    public void BindView(View view, Context context, IAttributeSet? attrs)
    {
        using (var typedArray = context.ObtainStyledAttributes(attrs, mvxAndroidBindingResource.Value.BindingStylableGroupId))
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

    private void ApplyBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
    {
        try
        {
            var bindingText = typedArray.GetString(attributeId);
            var newBindings = Binder?.Bind(_source, view, bindingText);
            StoreBindings(view, newBindings);
        }
        catch (Exception exception)
        {
            CrossBindingLogger.GetLogger<MvxAndroidViewBinder>().LogError(exception, $"Exception thrown during the view {view.GetType().FullName} binding");
        }
    }

    private void StoreBindings(View view, IEnumerable<ICrossUpdateableBinding>? newBindings)
    {
        if (newBindings != null)
        {
            _viewBindings.AddRange(newBindings.Select(b => new KeyValuePair<object, ICrossUpdateableBinding>(view, b)));
        }
    }

    private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
    {
        try
        {
            var bindingText = typedArray.GetString(attributeId);
            var newBindings = Binder?.LanguageBind(_source, view, bindingText);
            StoreBindings(view, newBindings);
        }
        catch (Exception exception)
        {
            CrossBindingLogger.GetLogger<MvxAndroidViewBinder>().LogError(exception, "Exception thrown during the view language binding");
            throw;
        }
    }
}
