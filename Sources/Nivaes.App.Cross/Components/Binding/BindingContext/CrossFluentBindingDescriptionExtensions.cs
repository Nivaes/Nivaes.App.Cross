using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

public static class CrossFluentBindingDescriptionExtensions
{
    extension<[DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(FluentBindingDescription<TTarget, TSource> bindingDescription) where TTarget : class
    {
        public FluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TFrom, TTo>(
            IDictionary<TFrom, TTo> converterParameter)
                where TFrom : notnull
        {
            var converter = ActivatorUtilities.CreateInstance<CrossDictionaryValueConverter<TFrom, TTo>>(IPlatformApplication.Current!.ServiceProvider);

            return bindingDescription.WithConversion(
                    converter, new Tuple<IDictionary<TFrom, TTo>, TTo?, bool>(
                    converterParameter, default, false))
                    .OneWay();
        }

        public FluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TFrom, TTo>(
                IDictionary<TFrom, TTo> converterParameter,
                TTo fallback)
                    where TFrom : notnull
        {
            var converter = ActivatorUtilities.CreateInstance<CrossDictionaryValueConverter<TFrom, TTo>>(IPlatformApplication.Current!.ServiceProvider);

            return bindingDescription.WithConversion(
                converter, new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
                .OneWay();
        }
    }
}
