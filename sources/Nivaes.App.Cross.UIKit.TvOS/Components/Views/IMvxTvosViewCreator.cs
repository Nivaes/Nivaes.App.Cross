namespace MvvmCross.Platforms.Tvos.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxTvosViewCreator : IMvxCurrentRequest
    {
        IMvxTvosView CreateView(MvxViewModelRequest request);

        IMvxTvosView CreateView(ICrossViewModel viewModel);

        IMvxTvosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, MvxViewModelRequest request);
    }
}
