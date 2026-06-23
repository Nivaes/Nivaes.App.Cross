namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class CrossDictionaryValueConverter<TKey, TValue> : CrossValueConverter<TKey, TValue>
        where TKey : notnull
    {
        protected override TValue Convert(TKey value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            
            ArgumentNullException.ThrowIfNull(parameter, $"Dictionary Converter expected a parameter of type \"{typeof(Tuple<IDictionary<TKey, TValue>, TValue, bool>)}\" but received null");

            try
            {
                var typedParameters = (Tuple<IDictionary<TKey, TValue>, TValue, bool>)parameter;

                if (typedParameters.Item1.ContainsKey(value))
                {
                    return typedParameters.Item1[value];
                }
                else if (typedParameters.Item3)
                {
                    return typedParameters.Item2;
                }

                throw new KeyNotFoundException($"Could not find key {value?.ToString()} for {typeof(CrossDictionaryValueConverter<TKey, TValue>)}.");
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException($"Dictionary Converter expected a parameter of type \"{typeof(Tuple<IDictionary<TKey, TValue>, TValue, bool>)}\" but received type \"{parameter.GetType()}\"", nameof(parameter), ex);
            }
        }
    }
}
