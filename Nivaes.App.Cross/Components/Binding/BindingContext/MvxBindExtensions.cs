// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvvmCross.Binding.Bindings;
using MvvmCross.Converters;

namespace MvvmCross.Binding.BindingContext
{
    public static class MvxBindExtensions
    {
        public static MvxInlineBindingTarget<TViewModel> CreateInlineBindingTarget<TViewModel>(
            this IMvxBindingContextOwner bindingContextOwner)
        {
            return new MvxInlineBindingTarget<TViewModel>(bindingContextOwner);
        }

        public static T Bind<T, TViewModel>(this T element, MvxInlineBindingTarget<TViewModel> target,
                                            string descriptionText)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.BindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string? converterName = null,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            return element.Bind(target, null, sourcePropertyPath, converterName, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IMvxValueConverter converter,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            return element.Bind(target, null, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel>? target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string? converterName = null,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            var converter = MvxBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return element.Bind(target, targetPropertyPath, sourcePropertyPath, converter, converterParameter,
                                fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IMvxValueConverter converter,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePath = parser.Parse(sourcePropertyPath).Print();
            var targetPath = targetPropertyPath == null ? null : parser.Parse(targetPropertyPath).Print();
            return element.Bind(target, targetPath, sourcePath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            string targetPath,
                                            string sourcePath,
                                            IMvxValueConverter? converter = null,
                                            object? converterParameter = null,
                                            object? fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (string.IsNullOrEmpty(targetPath))
                targetPath = MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(T));

            var bindingDescription = new MvxBindingDescription(
                targetPath,
                sourcePath,
                converter,
                converterParameter,
                fallbackValue,
                mode);

            target.BindingContextOwner.AddBinding(element, bindingDescription);

            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingContextOwner bindingContextOwner, string descriptionText)
        {
            if (element == null) throw new NullReferenceException(nameof(element));

            bindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingContextOwner bindingContextOwner,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            if (element == null) throw new NullReferenceException(nameof(element));

            bindingContextOwner.AddBindings(element, descriptions);
            return element;
        }
    }
}
