namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public static partial class CrossBindingContextOwnerExtensions
    {
        // note that we don't add more default parameters here
        // - otherwise this overrides the other existing methods
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(this ICrossBindingContextOwner owner
                                                 , TTarget target
                                                 , string sourceKey)
        {
            var targetPath = CrossBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
            owner.BindLanguage(target, targetPath, sourceKey);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(this ICrossBindingContextOwner owner
                                                 , TTarget target
                                                 , string sourceKey
                                                 , CrossBindingMode bindingMode)
        {
            var targetPath = CrossBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
            owner.BindLanguage(target, targetPath, sourceKey, bindingMode: bindingMode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TViewModel>(this ICrossBindingContextOwner owner
                                                             , TTarget target
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
        public static void BindLanguage<TTarget>(this ICrossBindingContextOwner owner
                                                 , TTarget target
                                                 , Expression<Func<TTarget, object>> targetPropertyExpression
                                                 , string sourceKey
                                                 , string sourcePropertyName = null
                                                 , string fallbackValue = null
                                                 , string converterName = null
                                                 , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
        {
            var parser = PropertyExpressionParser;
            var parsedTargetPath = parser.Parse(targetPropertyExpression);
            var parsedTargetPathText = parsedTargetPath.Print();
            owner.BindLanguage(target, parsedTargetPathText, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void BindLanguage<TTarget, TViewModel>(this ICrossBindingContextOwner owner
                                                             , TTarget target
                                                             ,
                                                             Expression<Func<TTarget, object>> targetPropertyExpression
                                                             , string sourceKey
                                                             ,
                                                             Expression<Func<TViewModel, ICrossLanguageBinder>> sourcePropertyExpression
                                                             , string fallbackValue = null
                                                             , string converterName = null
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
        public static void BindLanguage(this ICrossBindingContextOwner owner
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null
                                        , CrossBindingMode bindingMode = CrossBindingMode.OneTime)
        {
            owner.BindLanguage(owner, targetPropertyName, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void BindLanguage(this ICrossBindingContextOwner owner
                                        , object target
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null
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

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void AddLangBindings(this ICrossBindingContextOwner view, object target, string bindingText)
        {
            var bindings = Binder.LanguageBind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(target, bindings);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void AddLangBindings(this ICrossBindingContextOwner view, IDictionary<object, string> lookup)
        {
            foreach (var kvp in lookup)
                view.AddLangBindings(kvp.Key, kvp.Value);
        }
    }
}
