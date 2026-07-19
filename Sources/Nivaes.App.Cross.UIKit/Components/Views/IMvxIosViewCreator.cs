namespace Nivaes.App.Cross.UIKitLib
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public interface IMvxIosViewCreator
        : ICrossCurrentRequest
    {
        IMvxIosView CreateView(ViewModelRequest request);

        IMvxIosView CreateView(ICrossViewModel viewModel);

        IMvxIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType);
    }
}
