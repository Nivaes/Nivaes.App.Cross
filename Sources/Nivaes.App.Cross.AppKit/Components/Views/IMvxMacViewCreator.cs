namespace Nivaes.App.Cross.AppKitLib
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(ViewModelRequest request);

        IMvxMacView CreateView(ICrossViewModel viewModel);

        IMvxMacView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, ViewModelRequest request);
    }
}
