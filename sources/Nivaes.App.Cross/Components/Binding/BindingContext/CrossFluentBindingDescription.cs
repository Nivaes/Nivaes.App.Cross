using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

public class CrossFluentBindingDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>
    : CrossBaseFluentBindingDescription<TTarget>
    where TTarget : class
{
    public CrossFluentBindingDescription(ICrossBindingContextOwner? bindingContextOwner, TTarget? target)
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
        //var filler = IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueCombinerRegistryFiller>();
        //var combinerName = filler.FindName(typeof(TValueCombiner));

        //return ByCombining(combinerName, properties);

        if (Singleton<CrossCombinersManager>.Instance.TryGetValue(typeof(TValueCombiner), out var combiner))
        {
            return ByCombining(combiner, properties);
        }
        else
        {
            throw new CrossException($"Unregistered {typeof(TValueCombiner).FullName} type combiner.");
        }
    }

    public CrossFluentBindingDescription<TTarget, TSource> ByCombining<TValueCombiner>(params string[] properties)
        where TValueCombiner : ICrossValueCombiner
    {
        //var filler = IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueCombinerRegistryFiller>();
        //var combinerName = filler.FindName(typeof(TValueCombiner));

        if (Singleton<CrossCombinersManager>.Instance.TryGetValue(typeof(TValueCombiner), out var combiner))
        {
            return ByCombining(combiner, properties);
        }
        else
        {
            throw new CrossException($"Unregistered {typeof(TValueCombiner).FullName} type combiner.");
        }
    }

    public CrossFluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
    {
        return WithConversion(new CrossCommandParameterValueConverter(), parameter);
    }

    public CrossFluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                        object? converterParameter = null)
    {
        var converter = ValueConverterFromName(converterName);
        return WithConversion(converter, converterParameter);
    }

    public CrossFluentBindingDescription<TTarget, TSource> WithConversion(ICrossValueConverter converter,
                                                                        object? converterParameter = null)
    {
        SourceStepDescription.Converter = converter;
        SourceStepDescription.ConverterParameter = converterParameter;
        return this;
    }

    public CrossFluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object? converterParameter = null)
        where TValueConverter : ICrossValueConverter
    {
        //var filler = IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueConverterRegistryFiller>();
        //var converterName = filler.FindName(typeof(TValueConverter));
        if (Singleton<CrossConvertersManager>.Instance.TryGetValue(typeof(TValueConverter), out var converter))
        {
            return WithConversion(converter, converterParameter);
        }
        else
        {
            throw new CrossException($"Unregistered {typeof(TValueConverter).FullName} type converter.");
        }
    }

    public CrossFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
    {
        SourceStepDescription.FallbackValue = fallback;
        return this;
    }

    public CrossFluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
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
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingDescription)
            .ToList();

        if (newBindingDescription.Count > 1)
        {
            CrossBindingLogger.Instance?.LogWarning("More than one description found - only first will be used in: {BindingDescription}", bindingDescription);
        }

        return FullyDescribed(newBindingDescription.FirstOrDefault());
    }

    public CrossFluentBindingDescription<TTarget, TSource> FullyDescribed(CrossBindingDescription? description)
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
    public MvxFluentBindingDescription(ICrossBindingContextOwner bindingContextOwner, TTarget? target = null)
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
        return Mode(CrossBindingMode.TwoWay);
    }

    public MvxFluentBindingDescription<TTarget> OneWay()
    {
        return Mode(CrossBindingMode.OneWay);
    }

    public MvxFluentBindingDescription<TTarget> OneWayToSource()
    {
        return Mode(CrossBindingMode.OneWayToSource);
    }

    public MvxFluentBindingDescription<TTarget> OneTime()
    {
        return Mode(CrossBindingMode.OneTime);
    }

    public MvxFluentBindingDescription<TTarget> Mode(CrossBindingMode mode)
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
        return WithConversion(new CrossCommandParameterValueConverter(), parameter);
    }

    public MvxFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                               object? converterParameter = null)
    {
        var converter = ValueConverterFromName(converterName);
        return WithConversion(converter, converterParameter);
    }

    public MvxFluentBindingDescription<TTarget> WithConversion(ICrossValueConverter converter,
                                                               object? converterParameter)
    {
        SourceStepDescription.Converter = converter;
        SourceStepDescription.ConverterParameter = converterParameter;
        return this;
    }

    public MvxFluentBindingDescription<TTarget> WithConversion<TValueConverter>(object? converterParameter = null)
        where TValueConverter : ICrossValueConverter
    {
        //var filler = IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueConverterRegistryFiller>();
        //var converterName = filler.FindName(typeof(TValueConverter));
        if(Singleton<CrossConvertersManager>.Instance.TryGetValue(typeof(TValueConverter), out var converter))
        {
            return WithConversion(converter, converterParameter);
        }
        else
        {
            throw new CrossException($"Unregistered {typeof(TValueConverter).FullName} type converter.");
        }       
    }

    public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
    {
        SourceStepDescription.FallbackValue = fallback;
        return this;
    }

    public MvxFluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
    {
        var newBindingDescription =
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
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
            Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingDescription)
            .ToList();

        if (newBindingDescription.Count > 1)
        {
            CrossBindingLogger.Instance?.LogWarning(
                "More than one description found - only first will be used in: {BindingDescription}",
                bindingDescription);
        }

        return FullyDescribed(newBindingDescription.FirstOrDefault());
    }

    public MvxFluentBindingDescription<TTarget> FullyDescribed(CrossBindingDescription? description)
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
