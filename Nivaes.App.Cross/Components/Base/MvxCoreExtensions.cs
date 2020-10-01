﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using MvvmCross.IoC;

    public static class MvxCoreExtensions
    {
        // core implementation of ConvertToBoolean
        public static bool ConvertToBooleanCore(this object result)
        {
            if (result == null)
                return false;

            if (result is string s)
                return !string.IsNullOrEmpty(s);

            if (result is bool x)
                return x;

            var resultType = result.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        // core implementation of MakeSafeValue
        public static object MakeSafeValueCore(this Type propertyType, object? value)
        {
            if (propertyType == null) throw new NullReferenceException(nameof(propertyType));

            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType == typeof(string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.GetTypeInfo().IsEnum)
                {
                    safeValue = value is string s ? Enum.Parse(propertyType, s, true) : Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    safeValue = underlyingType == typeof(bool) ? value.ConvertToBooleanCore() : ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
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
