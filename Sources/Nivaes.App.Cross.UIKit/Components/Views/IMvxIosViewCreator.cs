using System;
using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxIosViewCreator
        : ICrossCurrentRequest
    {
        IMvxIosView CreateView(IViewModelRequest request);

        IMvxIosView CreateView(ICrossViewModel viewModel);

        IMvxIosView CreateViewOfType(Type viewType);
    }
}
