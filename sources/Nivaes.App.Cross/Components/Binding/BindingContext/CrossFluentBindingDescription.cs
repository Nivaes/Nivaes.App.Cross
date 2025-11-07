namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Microsoft.Extensions.Logging;

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
            return Mode(CrossBindingMode.TwoWay);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneWay()
        {
            return Mode(CrossBindingMode.OneWay);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneWayToSource()
        {
            return Mode(CrossBindingMode.OneWayToSource);
        }

        public CrossFluentBindingDescription<TTarget, TSource> OneTime()
        {
            return Mode(CrossBindingMode.OneTime);
        }

        public CrossFluentBindingDescription<TTarget, TSource> Mode(CrossBindingMode mode)
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

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(ICrossValueCombiner combiner, params Expression<Func<TSource, object>>[] properties)
        {
            SetCombiner(combiner, properties.Select(SourcePropertyPath).ToArray(), useParser: false);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining(ICrossValueCombiner combiner, params string[] properties)
        {
            SetCombiner(combiner, properties, useParser: true);
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params Expression<Func<TSource, object>>[] properties)
            where TValueCombiner : ICrossValueCombiner
        {
            throw new NotImplementedException();
            //var filler = Cross.IoCProvider.Resolve<ICrossValueCombinerRegistryFiller>();
            //var combinerName = filler.FindName(typeof(TValueCombiner));

            //return ByCombining(combinerName, properties);
        }

        public CrossFluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params string[] properties)
            where TValueCombiner : ICrossValueCombiner
        {
            throw new NotImplementedException();
            //var filler = Cross.IoCProvider.Resolve<ICrossValueCombinerRegistryFiller>();
            //var combinerName = filler.FindName(typeof(TValueCombiner));

            //return ByCombining(combinerName, properties);
        }

        public CrossFluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
        {
            return WithConversion(new CrossCommandParameterValueConverter(), parameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                            object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion(ICrossValueConverter converter,
                                                                            object converterParameter = null)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : ICrossValueConverter
        {
            throw new NotImplementedException();
            //var filler = Cross.IoCProvider.Resolve<ICrossValueConverterRegistryFiller>();
            //var converterName = filler.FindName(typeof(TValueConverter));

            //return WithConversion(converterName, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public CrossFluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
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
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                CrossBindingLog.Instance?.LogWarning("More than one description found - only first will be used in: {BindingDescription}", bindingDescription);
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

    public class CrossFluentBindingDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>
        : CrossBaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public CrossFluentBindingDescription(ICrossBindingContextOwner bindingContextOwner, TTarget target = null)
            : base(bindingContextOwner, target)
        {
        }

        public CrossFluentBindingDescription<TTarget> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public CrossFluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public CrossFluentBindingDescription<TTarget> TwoWay()
        {
            return Mode(CrossBindingMode.TwoWay);
        }

        public CrossFluentBindingDescription<TTarget> OneWay()
        {
            return Mode(CrossBindingMode.OneWay);
        }

        public CrossFluentBindingDescription<TTarget> OneWayToSource()
        {
            return Mode(CrossBindingMode.OneWayToSource);
        }

        public CrossFluentBindingDescription<TTarget> OneTime()
        {
            return Mode(CrossBindingMode.OneTime);
        }

        public CrossFluentBindingDescription<TTarget> Mode(CrossBindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public CrossFluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public CrossFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public CrossFluentBindingDescription<TTarget> CommandParameter(object parameter)
        {
            return WithConversion(new CrossCommandParameterValueConverter(), parameter);
        }

        public CrossFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget> WithConversion(ICrossValueConverter converter,
                                                                   object converterParameter)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public CrossFluentBindingDescription<TTarget> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : ICrossValueConverter
        {
            throw new NotImplementedException();

            //var filler = Cross.IoCProvider.Resolve<ICrossValueConverterRegistryFiller>();
            //var converterName = filler.FindName(typeof(TValueConverter));

            //return WithConversion(converterName, converterParameter);
        }

        public CrossFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public CrossFluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public CrossFluentBindingDescription<TTarget> SourceDescribed(CrossBindingDescription description)
        {
            SourceOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public CrossFluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                CrossBindingLog.Instance?.LogWarning(
                    "More than one description found - only first will be used in: {BindingDescription}",
                    bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public CrossFluentBindingDescription<TTarget> FullyDescribed(CrossBindingDescription description)
        {
            FullOverwrite(description ?? new CrossBindingDescription());
            return this;
        }

        public CrossFluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }
}
