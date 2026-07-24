namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Observability;

    public class MvxWindowsBindingCreator : MvxBindingCreator
    {
        protected virtual void ApplyBinding(CrossBindingDescription bindingDescription, Type actualType,
                                            FrameworkElement attachedObject)
        {
            DependencyProperty dependencyProperty = actualType.FindDependencyProperty(bindingDescription.TargetName);
            if (dependencyProperty == null)
            {
                CrossLoggerHost.GetLogger<MvxWindowsBindingCreator>().LogWarning("Dependency property not found for {targetName}", bindingDescription.TargetName);

                return;
            }

            var property = actualType.FindActualProperty(bindingDescription.TargetName);
            if (property == null)
            {
                CrossLoggerHost.GetLogger<MvxWindowsBindingCreator>().LogWarning("Property not returned for target {targetName} - may cause issues", bindingDescription.TargetName);
            }

            var sourceStep = bindingDescription.Source as CrossPathSourceStepDescription;
            if (sourceStep == null)
            {
                CrossLoggerHost.GetLogger<MvxWindowsBindingCreator>().LogWarning(
                    "Binding description for {targetName} is not a simple path - Windows Binding cannot cope with this", bindingDescription.TargetName);
                return;
            }

            var newBinding = new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath(sourceStep.SourcePropertyPath),
                Mode = ConvertMode(bindingDescription.Mode, property?.PropertyType ?? typeof(object)),
                Converter = GetConverter(sourceStep.Converter),
                ConverterParameter = sourceStep.ConverterParameter,
                FallbackValue = sourceStep.FallbackValue
            };

            BindingOperations.SetBinding(attachedObject, dependencyProperty, newBinding);
        }

        protected override void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            var actualType = attachedObject.GetType();
            foreach (var bindingDescription in bindingDescriptions)
            {
                ApplyBinding(bindingDescription, actualType, attachedObject);
            }
        }

        protected static IValueConverter? GetConverter(ICrossValueConverter converter)
        {
            if (converter == null)
                return null;

            // TODO - consider caching this wrapper - it is a tiny bit wasteful creating a wrapper for each binding
            return new NativeValueConverter(converter);
        }

        protected static BindingMode ConvertMode(CrossBindingMode mode, Type propertyType)
        {
            switch (mode)
            {
                case CrossBindingMode.Default:
                    // if we return TwoWay for ImageSource then we end up in
                    // problems with WP7 not doing the auto-conversion
                    // see some of my angst in http://stackoverflow.com/questions/16752242/how-does-xaml-create-the-string-to-bitmapimage-value-conversion-when-binding-to/16753488#16753488
                    // Note: if we discover other issues here, then we should make a more flexible solution
                    if (propertyType == typeof(ImageSource))
                        return BindingMode.OneWay;

                    return BindingMode.TwoWay;

                case CrossBindingMode.TwoWay:
                    return BindingMode.TwoWay;

                case CrossBindingMode.OneWay:
                    return BindingMode.OneWay;

                case CrossBindingMode.OneTime:
                    return BindingMode.OneTime;

                case CrossBindingMode.OneWayToSource:
                    return BindingMode.TwoWay;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }
    }
}
