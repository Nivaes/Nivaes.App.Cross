namespace MvvmCross.DroidX.RecyclerView.AttributeHelpers
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;
    using Android.Content.Res;
    using Android.Util;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.DroidX.RecyclerView.ItemTemplates;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;

    public static class MvxRecyclerViewAttributeExtensions
    {
        private static bool _areBindingResourcesInitialized;
        private static int[]? _recyclerViewItemTemplateSelectorGroupId;
        private static int _recyclerViewItemTemplateSelector;

        private static string ReadRecyclerViewItemTemplateSelectorClassName(Context context, IAttributeSet attrs)
        {
            if (!_areBindingResourcesInitialized)
            {
                if (!TryInitializeBindingResourcePaths(out _recyclerViewItemTemplateSelectorGroupId, out _recyclerViewItemTemplateSelector))
                {
                    _areBindingResourcesInitialized = true;
                    return string.Empty;
                }
                _areBindingResourcesInitialized = true;
            }

            TypedArray? typedArray = null;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, _recyclerViewItemTemplateSelectorGroupId);
                var numberOfStyles = typedArray.IndexCount;

                for (var i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);
                    if (attributeId != _recyclerViewItemTemplateSelector) continue;

                    var className = typedArray.GetString(attributeId);
                    if (!string.IsNullOrEmpty(className))
                        return className;
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            return string.Empty;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Template selector type names are provided by user in XML attributes. Types must be preserved through linker configuration.")]
        public static IMvxTemplateSelector? BuildItemTemplateSelector(Context context, IAttributeSet attrs, int itemTemplateId)
        {
            var templateSelectorClassName = ReadRecyclerViewItemTemplateSelectorClassName(context, attrs);
            var type = string.IsNullOrEmpty(templateSelectorClassName)
                ? typeof(MvxDefaultTemplateSelector)
                : Type.GetType(templateSelectorClassName);

            if (type == null)
            {
                string message =
                    @$"Type with class name: {templateSelectorClassName} does not exist.
                    Make sure you have provided full Type name: namespace + class name, AssemblyName.
                    Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";

                //var logger = IPlatformApplication.Current!.Services.GetRequiredService<ILoggerFactory>()
                //   .CreateLogger($"{nameof(MvxRecyclerViewAttributeExtensions)}.{nameof(BuildItemTemplateSelector)}");

                var logger = CrossLogHost.GetLogger();
                logger.Log(LogLevel.Error, message, templateSelectorClassName);
                throw new InvalidOperationException(message);
            }

            if (!typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                const string message = "Type: {Type} does not implement {TemplateSelectorType} interface.";
                var logger = IPlatformApplication.Current!.Services.GetRequiredService<ILogger>();
                logger.Log(LogLevel.Error, message, type, nameof(IMvxTemplateSelector));

                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                const string message = "Cannot instantiate {TemplateSelectorType} as provided type: {Type} is abstract/interface.";
                var logger = IPlatformApplication.Current!.Services.GetRequiredService<ILogger>();
                logger.Log(LogLevel.Error, message, nameof(IMvxTemplateSelector), type);

                throw new InvalidOperationException(message);
            }

            var templateSelector = (IMvxTemplateSelector?)Activator.CreateInstance(type);

            if (itemTemplateId != 0 && templateSelector != null)
                templateSelector.ItemTemplateId = itemTemplateId;

            return templateSelector;
        }

        private static bool TryInitializeBindingResourcePaths(out int[] selectorGroup, out int selector)
        {
            var logger = IPlatformApplication.Current!.Services.GetRequiredService<ILogger>();

            try
            {
                var styleableType = typeof(global::_Microsoft.Android.Resource.Designer.Resource).GetNestedType("Styleable");
                if (styleableType == null)
                {
                    logger.LogWarning("Could not find Styleable Type - MvxRecyclerView binding won't work correctly");
                    selectorGroup = [];
                    selector = 0;
                    return false;
                }
                logger.LogTrace("Styleable Type found: {Type}", styleableType.FullName);

                selectorGroup = (int[])(styleableType.GetProperty("MvxRecyclerView")?.GetValue(null) ?? Array.Empty<int>());
                selector = (int)(styleableType.GetProperty("MvxRecyclerView_MvxTemplateSelector")?.GetValue(null) ?? 0);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to initialize MvxRecyclerView binding resources");
            }

            selectorGroup = [];
            selector = 0;
            return false;
        }
    }
}
