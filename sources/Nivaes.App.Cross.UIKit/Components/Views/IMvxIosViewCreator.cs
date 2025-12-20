namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxIosViewCreator : IMvxCurrentRequest
    {
        IMvxIosView CreateView(MvxViewModelRequest request);

        IMvxIosView CreateView(ICrossViewModel viewModel);

        IMvxIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType);
    }
}
