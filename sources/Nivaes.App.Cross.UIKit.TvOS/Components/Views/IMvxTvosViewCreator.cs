namespace MvvmCross.Platforms.Tvos.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit.TvOS;

    public interface IMvxTvosViewCreator : IMvxCurrentRequest
    {
        IMvxTvosView CreateView(CrossViewModelRequest request);

        IMvxTvosView CreateView(ICrossViewModel viewModel);

        IMvxTvosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, CrossViewModelRequest request);
    }
}
