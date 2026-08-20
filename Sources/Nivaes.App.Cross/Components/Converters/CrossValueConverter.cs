using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossValueConverter
        : ICrossValueConverter
    {
        protected readonly ILogger Logger;

        public CrossValueConverter(ILogger logger)
        {
            Logger = logger;
        }

        public virtual object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return CrossBindingConstant.UnsetValue;
        }

        public virtual object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return CrossBindingConstant.UnsetValue;
        }
    }

    public abstract class CrossValueConverter<TFrom, TTo>
        : ICrossValueConverter
    {
        protected readonly ILogger Logger;

        public CrossValueConverter(ILogger logger)
        {
            Logger = logger;
        }

        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value!, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to Convert from {FromType} to {ToType}", typeof(TFrom), typeof(TTo));

                return CrossBindingConstant.UnsetValue;
            }
        }

        protected virtual TTo? Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return ConvertBack((TTo)value!, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to Convert from {FromType} to {ToType}", typeof(TFrom), typeof(TTo));

                return CrossBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom? ConvertBack(TTo value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CrossValueConverter<TFrom>
        : ICrossValueConverter
    {
        protected readonly ILogger Logger;

        public CrossValueConverter(ILogger logger)
        {
            Logger = logger;
        }

        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to Convert from {FromType}", typeof(TFrom));
                return CrossBindingConstant.UnsetValue;
            }
        }

        protected virtual object? Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object? ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return TypedConvertBack(value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to ConvertBack to {FromType}", typeof(TFrom));
                return CrossBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom? TypedConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}