namespace Nivaes.App.Cross
{
    public interface IPressenterAction
    {
        ValueTask<bool> ShowAction(Type viewType, IPresentationAttribute attribute, CrossViewModelRequest request);

        ValueTask<bool> CloseActon(ICrossViewModel viewModel, IPresentationAttribute attribute);
    }
}
