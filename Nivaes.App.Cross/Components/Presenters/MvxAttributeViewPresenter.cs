// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MvvmCross;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public abstract class MvxAttributeViewPresenter
        : MvxViewPresenter, IMvxAttributeViewPresenter
    {
        private IMvxViewModelTypeFinder? mViewModelTypeFinder;
        public virtual IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                return mViewModelTypeFinder ??= Mvx.IoCProvider.Resolve<IMvxViewModelTypeFinder>();
            }
            set
            {
                mViewModelTypeFinder = value;
            }
        }

        private IMvxViewsContainer? mViewsContainer;
        public virtual IMvxViewsContainer ViewsContainer
        {
            get
            {
                return mViewsContainer ??= Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            }
            set
            {
                mViewsContainer = value;
            }
        }

        private IDictionary<Type, MvxPresentationAttributeAction>? mAttributeTypesActionsDictionary;
        public virtual IDictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
        {
            get
            {
                if (mAttributeTypesActionsDictionary == null)
                {
                    mAttributeTypesActionsDictionary = new Dictionary<Type, MvxPresentationAttributeAction>();
                    RegisterAttributeTypes();
                }
                return mAttributeTypesActionsDictionary;
            }
            set
            {
                mAttributeTypesActionsDictionary = value;
            }
        }

        public abstract void RegisterAttributeTypes();

        public abstract MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

        public virtual MvxBasePresentationAttribute? GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterfaces().Contains(typeof(IMvxOverridePresentationAttribute)) ?? false)
            {
                var viewInstance = Activator.CreateInstance(viewType) as IMvxOverridePresentationAttribute;
                try
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);
                    if (presentationAttribute == null)
                    {
                        MvxLog.Instance?.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                        {
                            presentationAttribute.ViewType = viewType;
                        }

                        if (presentationAttribute.ViewModelType == null)
                        {
                            presentationAttribute.ViewModelType = request.ViewModelType;
                        }

                        return presentationAttribute;
                    }
                }
                finally
                {
                    (viewInstance as IDisposable)?.Dispose();
                }
            }

            return null;
        }

        public virtual MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            if (viewType
                .GetCustomAttributes(typeof(MvxBasePresentationAttribute), true)
                .FirstOrDefault() is MvxBasePresentationAttribute attribute)
            {
                if (attribute.ViewType == null)
                    attribute.ViewType = viewType;

                if (attribute.ViewModelType == null)
                    attribute.ViewModelType = request.ViewModelType;

                return attribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        protected virtual MvxPresentationAttributeAction GetPresentationAttributeAction(MvxViewModelRequest request, out MvxBasePresentationAttribute attribute)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            attribute = GetPresentationAttribute(request);
            attribute.ViewModelType = request.ViewModelType;
            var viewType = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");

                if (attributeAction.CloseAction == null)
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");

                return attributeAction;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        public override async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint).ConfigureAwait(false)) return true;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                return await Close(presentationHint.ViewModelToClose).ConfigureAwait(false);
            }

            MvxLog.Instance?.Warn("Hint ignored {0}", hint.GetType().Name);
            return false;
        }

        public override ValueTask<bool> Close(IMvxViewModel viewModel)
        {
            return GetPresentationAttributeAction(new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute).CloseAction!.Invoke(viewModel, attribute);
        }

        public override ValueTask<bool> Show(MvxViewModelRequest request)
        {
            return GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute).ShowAction!.Invoke(attribute.ViewType!, attribute, request);
        }
    }
}
