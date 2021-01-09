// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.IoC;
    using Nivaes.App.Cross.ViewModels;

    public static class MvxSimplePropertyDictionaryExtensions
    {
        public static IDictionary<string, string> ToSimpleStringPropertyDictionary(
            this IDictionary<string, object> input)
        {
            if (input == null) return new Dictionary<string, string>();

            return input.ToDictionary(x => x.Key, x => x.Value?.ToStringInvariant());
        }

        public static IDictionary<string, string> SafeGetData(this IMvxBundle bundle)
        {
            if (bundle == null) throw new NullReferenceException(nameof(bundle));

            return bundle.Data;
        }

        public static void Write(this IDictionary<string, string> data, object toStore)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (toStore == null)
                return;

            var propertyDictionary = toStore.ToSimplePropertyDictionary();
            foreach (var kvp in propertyDictionary)
            {
                data[kvp.Key] = kvp.Value;
            }
        }

        public static T Read<T>(this IDictionary<string, string> data)
            where T : new()
        {
            return (T)data.Read(typeof(T));
        }

        public static object Read(this IDictionary<string, string> data, Type type)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var t = Activator.CreateInstance(type);
            var propertyList =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite);

            foreach (var propertyInfo in propertyList)
            {
                if (!data.TryGetValue(propertyInfo.Name, out string? textValue))
                    continue;

                var parser = IoCContainer.ComponentContext.Resolve<IMvxStringToTypeParser>();
                var typedValue = parser.ReadValue(textValue, propertyInfo.PropertyType, propertyInfo.Name);

                propertyInfo.SetValue(t, typedValue, Array.Empty<object>());
            }

            return t;
        }

        public static IEnumerable<object> CreateArgumentList(this IDictionary<string, string> data,
                                                             IEnumerable<ParameterInfo> requiredParameters,
                                                             string debugText)
        {
            if (requiredParameters == null) throw new ArgumentNullException(nameof(requiredParameters));

            var argumentList = new List<object>();
            foreach (var requiredParameter in requiredParameters)
            {
                var argumentValue = data.GetArgumentValue(requiredParameter, debugText);
                argumentList.Add(argumentValue);
            }
            return argumentList;
        }

        public static object GetArgumentValue(this IDictionary<string, string> data, ParameterInfo requiredParameter, string debugText)
        {
            if (requiredParameter == null) throw new ArgumentNullException(nameof(requiredParameter));

            if (data == null ||
                !data.TryGetValue(requiredParameter.Name, out string? parameterValue))
            {
                if (requiredParameter.IsOptional)
                {
                    return Type.Missing;
                }

                //MvxLog.Instance?.Trace(
                //    "Missing parameter for call to {0} - missing parameter {1} - asssuming null - this may fail for value types!",
                //    debugText,
                //    requiredParameter.Name);
                parameterValue = null;
            }

            var parser = IoCContainer.ComponentContext.Resolve<IMvxStringToTypeParser>();
            var value = parser.ReadValue(parameterValue, requiredParameter.ParameterType, requiredParameter.Name);
            return value;
        }

        public static IDictionary<string, string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            if (input is IDictionary<string, string> dictionary2)
                return dictionary2;

            var parser = IoCContainer.ComponentContext.Resolve<IMvxStringToTypeParser>();

            var propertyInfos = from property in input.GetType()
                                                      .GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                                     BindingFlags.FlattenHierarchy)
                                where property.CanRead
                                select new
                                {
                                    CanSerialize =
                                    parser.TypeSupported(property.PropertyType),
                                    Property = property
                                };

            var dictionary = new Dictionary<string, string>();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.CanSerialize)
                {
                    dictionary[propertyInfo.Property.Name] = input.GetPropertyValueAsString(propertyInfo.Property);
                }
                //else
                //{
                //    MvxLog.Instance?.Trace(
                //        "Skipping serialization of property {0} - don't know how to serialize type {1} - some answers on http://stackoverflow.com/questions/16524236/custom-types-in-navigation-parameters-in-v3",
                //        propertyInfo.Property.Name,
                //        propertyInfo.Property.PropertyType.Name);
                //}
            }
            return dictionary;
        }

        public static string GetPropertyValueAsString(this object input, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            try
            {
                var value = propertyInfo.GetValue(input, new object[] { });
                return value?.ToStringInvariant();
            }
            catch (Exception ex)
            {
                throw new MvxException(
                    "Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1", ex);
            }
        }

        private static string ToStringInvariant(this object value)
        {
            switch (value)
            {
                case DateTime dateTime: return dateTime.ToString("o", CultureInfo.InvariantCulture);
                case double doubleValue: return doubleValue.ToString("r", CultureInfo.InvariantCulture);
                case float floatValue: return floatValue.ToString("r", CultureInfo.InvariantCulture);
                case IFormattable formattableValue: return formattableValue.ToString(null, CultureInfo.InvariantCulture);
                default: return value.ToString();
            }
        }
    }
}
