namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxIosViewCreator : ICrossCurrentRequest
    {
        IMvxIosView CreateView(CrossViewModelRequest request);

        IMvxIosView CreateView(ICrossViewModel viewModel);

        IMvxIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType);
    }
}
