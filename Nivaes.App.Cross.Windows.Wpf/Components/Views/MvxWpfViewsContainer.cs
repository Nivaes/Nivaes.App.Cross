// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf.Views
{
    using System;
    using System.Windows;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.ViewModels;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using System.Threading.Tasks;

    public class MvxWpfViewsContainer
        : MvxViewsContainer
        , IMvxWpfViewsContainer
    {
        public virtual async ValueTask<FrameworkElement> CreateView(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            var wpfView = await CreateView(viewType).ConfigureAwait(false) as IMvxWpfView;

            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                wpfView.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
                wpfView.ViewModel = await viewModelLoader.LoadViewModel(request, null).ConfigureAwait(false);
            }

            return wpfView as FrameworkElement;
        }

        public ValueTask<FrameworkElement> CreateView(Type viewType)
        {
            var viewObject = Activator.CreateInstance(viewType);
            if (viewObject == null)
                throw new MvxException("View not loaded for " + viewType);

            if (viewObject is not IMvxWpfView wpfView)
                throw new MvxException("Loaded View does not have IMvxWpfView interface " + viewType);

            if (viewObject is not FrameworkElement viewControl)
                throw new MvxException("Loaded View is not a FrameworkElement " + viewType);

            return new ValueTask<FrameworkElement>(viewControl);
        }
    }
}
