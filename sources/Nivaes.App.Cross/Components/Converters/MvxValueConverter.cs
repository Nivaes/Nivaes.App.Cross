namespace Nivaes.App.Cross
{
    using System.Globalization;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;

    public abstract class MvxValueConverter
        : IMvxValueConverter
    {
        public virtual object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return MvxBindingConstant.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return MvxBindingConstant.UnsetValue;
        }
    }

    public abstract class MvxValueConverter<TFrom, TTo>
        : IMvxValueConverter
    {
        public object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.LogError(e, "Failed to Convert from {FromType} to {ToType}", typeof(TFrom), typeof(TTo));
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TTo Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return ConvertBack((TTo)value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.LogError(e, "Failed to Convert from {FromType} to {ToType}", typeof(TFrom), typeof(TTo));
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom ConvertBack(TTo value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        private static ILogger? GetLog() => CrossLogHost.GetLog<MvxValueConverter>();
    }

    public abstract class MvxValueConverter<TFrom>
        : IMvxValueConverter
    {
        public object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                GetLog()?.LogError(e, "Failed to Convert from {FromType}", typeof(TFrom));
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual object Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return TypedConvertBack(value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.LogError(e, "Failed to ConvertBack to {FromType}", typeof(TFrom));
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom TypedConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        private static ILogger? GetLog() => CrossLogHost.GetLog<MvxValueConverter>();
    }
}