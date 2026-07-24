using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Nivaes.App.Cross.Components;

namespace Nivaes.App.Cross
{
    [Obsolete("1", true)]
    public static class CrossCoreExtensions
    {
        // core implementation of ConvertToBoolean
        public static bool ConvertToBooleanCore<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this T? result)
        {
            if (EqualityComparer<T?>.Default.Equals(result, default))
                return false;

            var s = result as string;
            if (s != null)
                return !string.IsNullOrEmpty(s);

            if (result is bool x)
                return x;

            var resultType = result!.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        // core implementation of MakeSafeValue
        public static object? MakeSafeValueCore(this Type propertyType, object? value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                System.Diagnostics.Debugger.Break();  //ToDo: ¿Cuando se ejecuta esto?
                if (propertyType == typeof(string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.GetTypeInfo().IsEnum)
                {
                    var s = value as string;
                    safeValue =
                        s != null ?
                            Enum.Parse(propertyType, s, true) :
                            Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    safeValue =
                        underlyingType == typeof(bool) ?
                            value.ConvertToBooleanCore() :
                            ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else
                {
                    safeValue = ErrorMaskedConvert(value, propertyType, CultureInfo.CurrentUICulture);
                }
            }
            return safeValue;
        }

        private static object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
        {
            try
            {
                return Convert.ChangeType(value, type, cultureInfo);
            }
            catch (Exception)
            {
                // pokemon - mask the error
                return value;
            }
        }
    }
}
