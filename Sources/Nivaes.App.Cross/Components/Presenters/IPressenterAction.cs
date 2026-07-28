namespace Nivaes.App.Cross
{
    public interface IPressenterAction
    {
        ValueTask<bool> ShowAction(IViewModelRequest request, IPresentationAttribute attribute);

        ValueTask<bool> CloseAction(IViewModelRequest request, IPresentationAttribute attribute);
    }
}
