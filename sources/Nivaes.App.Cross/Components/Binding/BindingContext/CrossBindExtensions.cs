namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public static class CrossBindExtensions
    {
        public static CrossInlineBindingTarget<TViewModel> CreateInlineBindingTarget<TViewModel>(
            this ICrossBindingContextOwner bindingContextOwner)
        {
            return new CrossInlineBindingTarget<TViewModel>(bindingContextOwner);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<T, TViewModel>(this T element, CrossInlineBindingTarget<TViewModel> target,
                                            string descriptionText)
        {
            target.BindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T, TViewModel>(this T element,
                                            CrossInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            CrossBindingMode mode = CrossBindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converterName, converterParameter, fallbackValue, mode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T, TViewModel>(this T element,
                                            CrossInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            ICrossValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            CrossBindingMode mode = CrossBindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T, TViewModel>(this T element,
                                            CrossInlineBindingTarget<TViewModel> target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            CrossBindingMode mode = CrossBindingMode.Default)
        {
            var converter = CrossBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return element.Bind(target, targetPropertyPath, sourcePropertyPath, converter, converterParameter,
                                fallbackValue, mode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T, TViewModel>(this T element,
                                            CrossInlineBindingTarget<TViewModel> target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            ICrossValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            CrossBindingMode mode = CrossBindingMode.Default)
        {
            var parser = CrossBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePath = parser.Parse(sourcePropertyPath).Print();
            var targetPath = targetPropertyPath == null ? null : parser.Parse(targetPropertyPath).Print();
            return element.Bind(target, targetPath, sourcePath, converter, converterParameter, fallbackValue, mode);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T, TViewModel>(this T element,
                                            CrossInlineBindingTarget<TViewModel> target,
                                            string targetPath,
                                            string sourcePath,
                                            ICrossValueConverter? converter = null,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            CrossBindingMode mode = CrossBindingMode.Default)
        {
            if (string.IsNullOrEmpty(targetPath))
                targetPath = CrossBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(T));

            var bindingDescription = new CrossBindingDescription(
                targetPath,
                sourcePath,
                converter,
                converterParameter,
                fallbackValue,
                mode);

            target.BindingContextOwner.AddBinding(element, bindingDescription);

            return element;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<T>(this T element, ICrossBindingContextOwner bindingContextOwner, string descriptionText)
        {
            bindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static T Bind<T>(this T element, ICrossBindingContextOwner bindingContextOwner,
                                IEnumerable<CrossBindingDescription> descriptions)
        {
            bindingContextOwner.AddBindings(element, descriptions);
            return element;
        }
    }
}
