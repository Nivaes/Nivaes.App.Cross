using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Nivaes.App.Cross.Components;

namespace Nivaes.App.Cross
{
    public static class CrossBindingExtensions
    {
        extension(ICrossEditableTextView editableTextView)
        {
            public bool ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(object target, object? value)
            {
                if (value == null)
                    return false;

                // specifically for int, double, float and decimal we do some special comparisons
                // to prevent the user losing trailing periods, leading minus signs, leading zeroes and trailing zeros
                var valueType = value.GetType();
                if (valueType == typeof(int) ||
                    valueType == typeof(double) ||
                    valueType == typeof(float) ||
                    valueType == typeof(decimal))
                {
                    var currentValue = editableTextView.CurrentText;
                    if (currentValue == null)
                        return false;

                    try
                    {
                        var equivalentCurrentValue = valueType.MakeSafeValue(currentValue);
                        if (equivalentCurrentValue?.Equals(value) == true)
                            return true;
                    }
                    catch (FormatException)
                    {
                        // format problem - so they are definitely not equivalent
                        return false;
                    }
                }

                return false;
            }
        }

        public static bool ConvertToBoolean(this object? result)
        {
            return result.ConvertToBooleanCore();
        }

        [Obsolete("Si se usa hay que implementar en ConvertersContainers, para poder buscar por dos valores.")]
        public static object? MakeSafeValue(this Type propertyType, object? value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            throw new NotImplementedException("Si llega hasta aquí, mirar como se buscan converter con dos valores");
            //if(Singleton<ConvertersContainers>.Instance.Converters.TryGetValue((value.GetType(), propertyType), out var autoConverter))
            //{
            //    return autoConverter.Convert(value, propertyType, null, CultureInfo.CurrentUICulture);
            //}

            return propertyType.MakeSafeValueCore(value);
        }
    }
}