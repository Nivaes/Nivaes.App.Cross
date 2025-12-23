namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Binding;

    public class CrossFluentBindingDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>
        : CrossBaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public CrossFluentBindingDescription(ICrossBindingContextOwner bindingContextOwner, TTarget target)
            : base(bindingContextOwner, target)
        {
        }

        public CrossFluentBindingDescription<TTarget, TSource> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public CrossFluentBindingDescription<TTarget, TSource> TwoWay()
        {
            return Mode(MvxBindingMode.TwoWay);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneWay()
        {
            return Mode(MvxBindingMode.OneWay);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneWayToSource()
        {
            return Mode(MvxBindingMode.OneWayToSource);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneTime()
        {
            return Mode(MvxBindingMode.OneTime);
        }

        public CrossFluentBindingDescription<TTarget, TSource> Mode(MvxBindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params Expression<Func<TSource, object>>[] properties)
            => ByCombining(combinerName, properties.Select(SourcePropertyPath).ToArray());

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params string[] properties)
            => To($"{combinerName}({string.Join(", ", properties)})");

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(IMvxValueCombiner combiner, params Expression<Func<TSource, object>>[] properties)
        {
            SetCombiner(combiner, properties.Select(SourcePropertyPath).ToArray(), useParser: false);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(IMvxValueCombiner combiner, params string[] properties)
        {
            SetCombiner(combiner, properties, useParser: true);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params Expression<Func<TSource, object>>[] properties)
            where TValueCombiner : IMvxValueCombiner
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxValueCombinerRegistryFiller>();
            var combinerName = filler.FindName(typeof(TValueCombiner));

            return ByCombining(combinerName, properties);
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params string[] properties)
            where TValueCombiner : IMvxValueCombiner
        {
            var filler = Mvx.IoCProvider.Resolve<IMvxValueCombinerRegistryFiller>();
            var combinerName = filler.FindName(typeof(TValueCombiner));

            return ByCombining(combinerName, properties);
        }

        public CrossFluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
        {
            return WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                            object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion(IMvxValueConverter converter,
                                                                            object converterParameter = null)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IMvxValueConverter
        {
            var filler = Mvx.IoCProvider.Resolve<ICrossValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public CrossFluentBindingDescription<TTarget, TSource> SourceDescribed(CrossBindingDescription description)
        {
            SourceOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxBindingLog.Instance?.LogWarning("More than one description found - only first will be used in: {BindingDescription}", bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public CrossFluentBindingDescription<TTarget, TSource> FullyDescribed(CrossBindingDescription description)
        {
            FullOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }

    public class MvxFluentBindingDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>
        : CrossBaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public MvxFluentBindingDescription(ICrossBindingContextOwner bindingContextOwner, TTarget target = null)
            : base(bindingContextOwner, target)
        {
        }

        public MvxFluentBindingDescription<TTarget> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public MvxFluentBindingDescription<TTarget> TwoWay()
        {
            return Mode(MvxBindingMode.TwoWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWay()
        {
            return Mode(MvxBindingMode.OneWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWayToSource()
        {
            return Mode(MvxBindingMode.OneWayToSource);
        }

        public MvxFluentBindingDescription<TTarget> OneTime()
        {
            return Mode(MvxBindingMode.OneTime);
        }

        public MvxFluentBindingDescription<TTarget> Mode(MvxBindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> CommandParameter(object parameter)
        {
            return WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(IMvxValueConverter converter,
                                                                   object converterParameter)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IMvxValueConverter
        {
            var filler = Mvx.IoCProvider.Resolve<ICrossValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(CrossBindingDescription description)
        {
            SourceOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxBindingLog.Instance?.LogWarning(
                    "More than one description found - only first will be used in: {BindingDescription}",
                    bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(CrossBindingDescription description)
        {
            FullOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }
}
