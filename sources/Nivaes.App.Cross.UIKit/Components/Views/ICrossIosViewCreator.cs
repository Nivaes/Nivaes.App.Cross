using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosViewCreator : ICrossCurrentRequest
    {
        ICrossIosView CreateView(ICrossViewModelRequest request);

        ICrossIosView CreateView(ICrossViewModel viewModel);

        ICrossIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType);
    }
}
