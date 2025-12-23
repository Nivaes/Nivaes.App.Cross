namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using MvvmCross.Localization;

    public static partial class CrossBindingContextOwnerExtensions
    {
        extension(ICrossBindingContextOwner owner)
        {
            // note that we don't add more default parameters here
            // - otherwise this overrides the other existing methods
            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(TTarget target
                                                     , string sourceKey)
            {
                var targetPath = CrossBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
                owner.BindLanguage(target, targetPath, sourceKey);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(TTarget target
                                                     , string sourceKey
                                                     , CrossBindingMode bindingMode)
            {
                var targetPath = CrossBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
                owner.BindLanguage(target, targetPath, sourceKey, bindingMode: bindingMode);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TViewModel>(TTarget target
                                                                 , string sourceKey
                                                                 , Expression<Func<TViewModel, ICrossTextProvider>> textProvider
                                                                 , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
            {
                var parser = PropertyExpressionParser;
                var targetPath = CrossBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
                var sourcePath = parser.Parse(textProvider).Print();
                owner.BindLanguage(target, targetPath, sourceKey, sourcePath, bindingMode: bindingMode);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage<TTarget>(TTarget target
                                                     , Expression<Func<TTarget, object>> targetPropertyExpression
                                                     , string sourceKey
                                                     , string? sourcePropertyName = null
                                                     , string? fallbackValue = null
                                                     , string? converterName = null
                                                     , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
            {
                var parser = PropertyExpressionParser;
                var parsedTargetPath = parser.Parse(targetPropertyExpression);
                var parsedTargetPathText = parsedTargetPath.Print();
                owner.BindLanguage(target, parsedTargetPathText, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage<TTarget, TViewModel>(TTarget target
                                                          , Expression<Func<TTarget, object>> targetPropertyExpression
                                                          , string sourceKey
                                                          , Expression<Func<TViewModel, ICrossLanguageBinder>> sourcePropertyExpression
                                                          , string? fallbackValue = null
                                                          , string? converterName = null
                                                          , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
            {
                var parser = PropertyExpressionParser;
                var parsedTargetPath = parser.Parse(targetPropertyExpression);
                var parsedTargetPathText = parsedTargetPath.Print();
                var parsedSourcePath = parser.Parse(sourcePropertyExpression);
                var sourcePropertyName = parsedSourcePath.Print();
                owner.BindLanguage(target, parsedTargetPathText, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage(string targetPropertyName
                                            , string sourceKey
                                            , string? sourcePropertyName = null
                                            , string? fallbackValue = null
                                            , string? converterName = null
                                            , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
            {
                owner.BindLanguage(owner, targetPropertyName, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void BindLanguage(object? target
                                    , string? targetPropertyName
                                    , string sourceKey
                                    , string? sourcePropertyName = null
                                    , string? fallbackValue = null
                                    , string? converterName = null
                                    , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
            {
                converterName ??= LanguageParser.DefaultConverterName;
                sourcePropertyName ??= LanguageParser.DefaultTextSourceName;

                var converter = ValueConverterLookup.Find(converterName);

                var bindingDescription = new CrossBindingDescription
                {
                    TargetName = targetPropertyName,
                    Source = new CrossPathSourceStepDescription
                    {
                        SourcePropertyPath = sourcePropertyName,
                        Converter = converter,
                        ConverterParameter = sourceKey,
                        FallbackValue = fallbackValue,
                    },
                    Mode = bindingMode
                };
                owner.AddBinding(target, bindingDescription);
            }
        }

        extension(ICrossBindingContextOwner view)
        {
            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void AddLangBindings(object target, string bindingText)
            {
                var bindings = Binder.LanguageBind(view.BindingContext?.DataContext, target, bindingText);
                view.AddBindings(target, bindings);
            }

            [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
            public void AddLangBindings(IDictionary<object, string> lookup)
            {
                foreach (var kvp in lookup)
                    view.AddLangBindings(kvp.Key, kvp.Value);
            }
        }
    }
}
