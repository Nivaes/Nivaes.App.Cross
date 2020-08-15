﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color
{
    public abstract class MvxColorValueConverter : MvxValueConverter
    {
        private IMvxNativeColor? mNativeColor;

        private IMvxNativeColor NativeColor => mNativeColor ??= Mvx.IoCProvider.Resolve<IMvxNativeColor>();

        protected abstract System.Drawing.Color Convert(object? value, object? parameter, CultureInfo culture);

        public sealed override object? Convert(object? value, Type targetType, object? parameter,
                                       CultureInfo culture)
        {
            return NativeColor.ToNative(Convert(value, parameter, culture));
        }
    }

    public abstract class MvxColorValueConverter<T> : MvxColorValueConverter
    {
        protected sealed override System.Drawing.Color Convert(object? value, object? parameter, CultureInfo culture)
        {
            if (value is T t)
                return Convert(t, parameter, culture);

            return default;
        }

        protected abstract System.Drawing.Color Convert(T value, object? parameter, CultureInfo culture);
    }
}
