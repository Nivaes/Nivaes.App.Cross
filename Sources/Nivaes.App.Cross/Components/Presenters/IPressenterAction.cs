namespace Nivaes.App.Cross
{
    public interface IPressenterAction
    {
        ValueTask<bool> ShowAction(Type viewType, IPresentationAttribute attribute, ViewModelRequest request);

        ValueTask<bool> CloseAction(ICrossViewModel viewModel, IPresentationAttribute attribute);
    }
}
