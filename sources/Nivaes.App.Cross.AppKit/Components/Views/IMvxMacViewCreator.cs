namespace MvvmCross.Platforms.Mac.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(MvxViewModelRequest request);

        IMvxMacView CreateView(ICrossViewModel viewModel);

        IMvxMacView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, MvxViewModelRequest request);
    }
}
