using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

[Obsolete("", true)]
public static class CrossFluentBindingDescriptionExtensions
{
    [Obsolete("", true)]
    public static CrossFluentBindingDescription<TTarget, TSource> ToLocalizationId<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
            this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
            string localizationId)
                where TSource : ICrossLocalizedTextSourceOwner
                where TTarget : class
    {
        var valueConverter = IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueConverterLookup>().Find("Language");
        return bindingDescription.To(vm => vm.LocalizedTextSource)
            .OneTime()
            .WithConversion(valueConverter, localizationId);
    }

    public static CrossFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
            this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter)
                where TFrom : notnull
                where TTarget : class
    {
        var converter = ActivatorUtilities.CreateInstance<CrossDictionaryValueConverter<TFrom, TTo>>(IPlatformApplication.Current!.Services);

        return bindingDescription.WithConversion(
                converter, new Tuple<IDictionary<TFrom, TTo>, TTo?, bool>(
                converterParameter, default, false))
                .OneWay();
    }

    public static CrossFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
            this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter,
            TTo fallback)
                where TFrom : notnull
                where TTarget : class
    {
        var converter = ActivatorUtilities.CreateInstance<CrossDictionaryValueConverter<TFrom, TTo>>(IPlatformApplication.Current!.Services);

        return bindingDescription.WithConversion(
            converter, new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
            .OneWay();
    }
}
