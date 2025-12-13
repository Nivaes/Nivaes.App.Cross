namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection.Metadata;

    public static class CrossFluentBindingDescriptionExtensions
    {
        public static CrossFluentBindingDescription<TTarget, TSource> ToLocalizationId<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
                this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
                string localizationId)
                    where TSource : ICrossLocalizedTextSourceOwner
                    where TTarget : class
        {
            throw new NotImplementedException();
            //var valueConverter = Cross.IoCProvider.Resolve<ICrossValueConverterLookup>().Find("Language");
            //return bindingDescription.To(vm => vm.LocalizedTextSource)
            //    .OneTime()
            //    .WithConversion(valueConverter, localizationId);
        }

        public static CrossFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
                this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
                IDictionary<TFrom, TTo> converterParameter)
                    where TTarget : class
        {
            return bindingDescription.WithConversion(
                    new CrossDictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(
                        converterParameter, default, false))
                        .OneWay();
        }

        public static CrossFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
                this CrossFluentBindingDescription<TTarget, TSource> bindingDescription,
                IDictionary<TFrom, TTo> converterParameter,
                TTo fallback)
                    where TTarget : class
                => bindingDescription.WithConversion(
                    new CrossDictionaryValueConverter<TFrom, TTo>(),
                    new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
                    .OneWay();
    }
}
