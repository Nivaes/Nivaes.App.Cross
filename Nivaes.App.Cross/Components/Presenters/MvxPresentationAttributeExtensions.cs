// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public static class MvxPresentationAttributeExtensions
    {
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            if (candidateType == null) throw new ArgumentNullException(nameof(candidateType));

            var attributes = candidateType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<MvxBasePresentationAttribute> GetBasePresentationAttributes(this Type fromViewType)
        {
            if (fromViewType == null) throw new ArgumentNullException(nameof(fromViewType));

            var attributes = fromViewType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);

            if (attributes.Length == 0)
                throw new InvalidOperationException($"Type does not have {nameof(MvxBasePresentationAttribute)} attribute!");

            return attributes.Cast<MvxBasePresentationAttribute>();
        }

        public static MvxBasePresentationAttribute GetBasePresentationAttribute(this Type fromViewType)
        {
            if (fromViewType == null) throw new ArgumentNullException(nameof(fromViewType));

            return fromViewType.GetBasePresentationAttributes().FirstOrDefault();
        }

        public static Type? GetViewModelType(this Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            if (!viewType.HasBasePresentationAttribute())
                return null;

            return viewType.GetBasePresentationAttributes()
                .Select(x => x.ViewModelType)
                .FirstOrDefault();
        }

        public static void Register<TMvxPresentationAttribute>(
            this IDictionary<Type, MvxPresentationAttributeAction> attributeTypesToActionsDictionary,
            Func<Type, TMvxPresentationAttribute, MvxViewModelRequest, ValueTask<bool>> showAction,
            Func<IMvxViewModel, TMvxPresentationAttribute, ValueTask<bool>> closeAction) where TMvxPresentationAttribute : class, IMvxPresentationAttribute
        {
            if (attributeTypesToActionsDictionary == null) throw new ArgumentNullException(nameof(attributeTypesToActionsDictionary));

            attributeTypesToActionsDictionary.Add(
                typeof(TMvxPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => showAction(view, attribute as TMvxPresentationAttribute, request),
                    CloseAction = (viewModel, attribute) => closeAction(viewModel, attribute as TMvxPresentationAttribute)
                });
        }
    }
}
