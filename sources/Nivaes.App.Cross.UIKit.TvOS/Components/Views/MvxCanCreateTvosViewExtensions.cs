// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Core;
using MvvmCross.ViewModels;
using Nivaes.App.Cross;

namespace MvvmCross.Platforms.Tvos.Views
{
    public static class MvxCanCreateTvosViewExtensions
    {
        public static IMvxTvosView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            object parameterObject)
                where TTargetViewModel : class, ICrossViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxTvosView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            IDictionary<string, string> parameterValues = null)
                where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new CrossBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            CrossViewModelRequest request)
                where TTargetViewModel : class, ICrossViewModel
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            CrossViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewtype,
            CrossViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateViewOfType(viewtype, request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            ICrossViewModel viewModel)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(viewModel);
        }
    }
}
