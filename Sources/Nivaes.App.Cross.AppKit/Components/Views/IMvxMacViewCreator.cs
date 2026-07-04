namespace Nivaes.App.Cross.AppKitLib
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(CrossViewModelRequest request);

        IMvxMacView CreateView(ICrossViewModel viewModel);

        IMvxMacView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, CrossViewModelRequest request);
    }
}
