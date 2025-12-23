namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Localization;

    public static class MvxFluentBindingDescriptionExtensions
    {
        public static MvxFluentBindingDescription<TTarget, TSource> ToLocalizationId<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
                this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
                string localizationId)
                    where TSource : IMvxLocalizedTextSourceOwner
                    where TTarget : class
        {
            var valueConverter = Mvx.IoCProvider.Resolve<IMvxValueConverterLookup>().Find("Language");
            return bindingDescription.To(vm => vm.LocalizedTextSource)
                .OneTime()
                .WithConversion(valueConverter, localizationId);
        }

        public static MvxFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
                this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
                IDictionary<TFrom, TTo> converterParameter)
                    where TTarget : class
                => bindingDescription.WithConversion(
                    new MvxDictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(
                        converterParameter, default, false))
                        .OneWay();

        public static MvxFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource, TFrom, TTo>(
                this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
                IDictionary<TFrom, TTo> converterParameter,
                TTo fallback)
                    where TTarget : class
                => bindingDescription.WithConversion(
                    new MvxDictionaryValueConverter<TFrom, TTo>(),
                    new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
                    .OneWay();
    }
}
