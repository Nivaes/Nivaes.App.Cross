using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;

namespace Nivaes.App.Cross;

public class FluentBindingDescription<TTarget, TSource>
    : CrossBaseFluentBindingDescription<TTarget>
    where TTarget : class
{
    public FluentBindingDescription(ICrossBindingContextOwner? bindingContextOwner, TTarget? target)
        : base(bindingContextOwner, target)
    {
    }

    public FluentBindingDescription<TTarget, TSource> For(string targetPropertyName)
    {
        BindingDescription.TargetName = targetPropertyName;
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> For(Expression<Func<TTarget, object>> targetPropertyPath)
    {
        var targetPropertyName = TargetPropertyName(targetPropertyPath);
        return For(targetPropertyName);
    }

    public FluentBindingDescription<TTarget, TSource> TwoWay()
    {
        return Mode(CrossBindingMode.TwoWay);
    }

    public FluentBindingDescription<TTarget, TSource> OneWay()
    {
        return Mode(CrossBindingMode.OneWay);
    }

    public FluentBindingDescription<TTarget, TSource> OneWayToSource()
    {
        return Mode(CrossBindingMode.OneWayToSource);
    }

    public FluentBindingDescription<TTarget, TSource> OneTime()
    {
        return Mode(CrossBindingMode.OneTime);
    }

    public FluentBindingDescription<TTarget, TSource> Mode(CrossBindingMode mode)
    {
        BindingDescription.Mode = mode;
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> To(string sourcePropertyPath)
    {
        SetFreeTextPropertyPath(sourcePropertyPath);
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
    {
        var sourcePropertyPath = SourcePropertyPath(sourceProperty);
        SetKnownTextPropertyPath(sourcePropertyPath);
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params Expression<Func<TSource, object>>[] properties)
        => ByCombining(combinerName, properties.Select(SourcePropertyPath).ToArray());

    public FluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params string[] properties)
        => To($"{combinerName}({string.Join(", ", properties)})");

    public FluentBindingDescription<TTarget, TSource> ByCombining(ICrossValueCombiner combiner, params Expression<Func<TSource, object>>[] properties)
    {
        SetCombiner(combiner, properties.Select(SourcePropertyPath).ToArray(), useParser: false);
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> ByCombining(ICrossValueCombiner combiner, params string[] properties)
    {
        SetCombiner(combiner, properties, useParser: true);
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params Expression<Func<TSource, object>>[] properties)
        where TValueCombiner : ICrossValueCombiner
    {
        if (Singleton<CombinersContainers>.Instance.Combiners.TryGetValue(typeof(TValueCombiner), out var combiner))
        {
            return ByCombining(combiner, properties);
        }
        else
        {
            throw new AppException($"Unregistered {typeof(TValueCombiner).FullName} type combiner.");
        }
    }

    public FluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params string[] properties)
        where TValueCombiner : ICrossValueCombiner
    {
        if (Singleton<CombinersContainers>.Instance.Combiners.TryGetValue(typeof(TValueCombiner), out var combiner))
        {
            return ByCombining(combiner, properties);
        }
        else
        {
            throw new AppException($"Unregistered {typeof(TValueCombiner).FullName} type combiner.");
        }
    }

    public FluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
    {
        var converter = Singleton<ConvertersContainers>.Instance.Converters[typeof(CrossCommandParameterValueConverter)];

        return WithConversion(converter, parameter);
    }

    public FluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                        object? converterParameter = null)
    {
        var converter = ValueConverterFromName(converterName);
        return WithConversion(converter, converterParameter);
    }

    public FluentBindingDescription<TTarget, TSource> WithConversion(ICrossValueConverter converter,
                                                                        object? converterParameter = null)
    {
        SourceStepDescription.Converter = converter;
        SourceStepDescription.ConverterParameter = converterParameter;
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object? converterParameter = null)
        where TValueConverter : ICrossValueConverter
    {
        if (Singleton<ConvertersContainers>.Instance.Converters.TryGetValue(typeof(TValueConverter), out var converter))
        {
            return WithConversion(converter, converterParameter);
        }
        else
        {
            throw new AppException($"Unregistered {typeof(TValueConverter).FullName} type converter.");
        }
    }

    public FluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
    {
        SourceStepDescription.FallbackValue = fallback;
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
        return SourceDescribed(newBindingDescription);
    }

    public FluentBindingDescription<TTarget, TSource> SourceDescribed(CrossBindingDescription description)
    {
        SourceOverwrite(description ?? new CrossBindingDescription());
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> FullyDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingDescription)
            .ToList();

        if (newBindingDescription.Count > 1)
        {
            CrossBindingLogger.GetLogger<FluentBindingDescription<TTarget, TSource>>().LogWarning("More than one description found - only first will be used in: {BindingDescription}", bindingDescription);
        }

        return FullyDescribed(newBindingDescription.FirstOrDefault());
    }

    public FluentBindingDescription<TTarget, TSource> FullyDescribed(CrossBindingDescription? description)
    {
        FullOverwrite(description ?? new CrossBindingDescription());
        return this;
    }

    public FluentBindingDescription<TTarget, TSource> WithClearBindingKey(object clearBindingKey)
    {
        ClearBindingKey = clearBindingKey;
        return this;
    }
}

public class FluentBindingDescription<TTarget>
    : CrossBaseFluentBindingDescription<TTarget>
    where TTarget : class
{
    public FluentBindingDescription(ICrossBindingContextOwner bindingContextOwner, TTarget? target = null)
        : base(bindingContextOwner, target)
    {
    }

    public FluentBindingDescription<TTarget> For(string targetPropertyName)
    {
        BindingDescription.TargetName = targetPropertyName;
        return this;
    }

    public FluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
    {
        var targetPropertyName = TargetPropertyName(targetPropertyPath);
        return For(targetPropertyName);
    }

    public FluentBindingDescription<TTarget> TwoWay()
    {
        return Mode(CrossBindingMode.TwoWay);
    }

    public FluentBindingDescription<TTarget> OneWay()
    {
        return Mode(CrossBindingMode.OneWay);
    }

    public FluentBindingDescription<TTarget> OneWayToSource()
    {
        return Mode(CrossBindingMode.OneWayToSource);
    }

    public FluentBindingDescription<TTarget> OneTime()
    {
        return Mode(CrossBindingMode.OneTime);
    }

    public FluentBindingDescription<TTarget> Mode(CrossBindingMode mode)
    {
        BindingDescription.Mode = mode;
        return this;
    }

    public FluentBindingDescription<TTarget> To(string sourcePropertyPath)
    {
        SetFreeTextPropertyPath(sourcePropertyPath);
        return this;
    }

    public FluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
    {
        var sourcePropertyPath = SourcePropertyPath(sourceProperty);
        SetKnownTextPropertyPath(sourcePropertyPath);
        return this;
    }

    public FluentBindingDescription<TTarget> CommandParameter(object parameter)
    {
        var converter = Singleton<ConvertersContainers>.Instance.Converters[typeof(CrossCommandParameterValueConverter)];

        return WithConversion(converter, parameter);
    }

    public FluentBindingDescription<TTarget> WithConversion(string converterName,
                                                               object? converterParameter = null)
    {
        var converter = ValueConverterFromName(converterName);
        return WithConversion(converter, converterParameter);
    }

    public FluentBindingDescription<TTarget> WithConversion(ICrossValueConverter converter,
                                                               object? converterParameter)
    {
        SourceStepDescription.Converter = converter;
        SourceStepDescription.ConverterParameter = converterParameter;
        return this;
    }

    public FluentBindingDescription<TTarget> WithConversion<TValueConverter>(object? converterParameter = null)
        where TValueConverter : ICrossValueConverter
    {
        if (Singleton<ConvertersContainers>.Instance.Converters.TryGetValue(typeof(TValueConverter), out var converter))
        {
            return WithConversion(converter, converterParameter);
        }
        else
        {
            throw new AppException($"Unregistered {typeof(TValueConverter).FullName} type converter.");
        }
    }

    public FluentBindingDescription<TTarget> WithFallback(object fallback)
    {
        SourceStepDescription.FallbackValue = fallback;
        return this;
    }

    public FluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
        return SourceDescribed(newBindingDescription);
    }

    public FluentBindingDescription<TTarget> SourceDescribed(CrossBindingDescription description)
    {
        SourceOverwrite(description ?? new CrossBindingDescription());
        return this;
    }

    public FluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingDescription)
            .ToList();

        if (newBindingDescription.Count > 1)
        {
            CrossBindingLogger.GetLogger<FluentBindingDescription<TTarget>>().LogWarning(
                "More than one description found - only first will be used in: {BindingDescription}",
                bindingDescription);
        }

        return FullyDescribed(newBindingDescription.FirstOrDefault());
    }

    public FluentBindingDescription<TTarget> FullyDescribed(CrossBindingDescription? description)
    {
        FullOverwrite(description ?? new CrossBindingDescription());
        return this;
    }

    public FluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
    {
        ClearBindingKey = clearBindingKey;
        return this;
    }
}
