using System;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.AppKitLib
{
    public interface IMvxMacViewCreator
    {
        IMvxMacView CreateView(IViewModelRequest request);

        IMvxMacView CreateView(ICrossViewModel viewModel);

        IMvxMacView CreateViewOfType(Type viewType, IViewModelRequest request);
    }
}
