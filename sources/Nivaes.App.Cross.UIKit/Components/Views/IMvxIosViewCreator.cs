namespace Nivaes.App.Cross.UIKitOS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public interface IMvxIosViewCreator 
        : ICrossCurrentRequest
    {
        IMvxIosView CreateView(CrossViewModelRequest request);

        IMvxIosView CreateView(ICrossViewModel viewModel);

        IMvxIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType);
    }
}
