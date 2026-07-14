namespace Nivaes.App.Cross
{
    public  interface IPressenterAction
    {
        public abstract ValueTask<bool> ShowActon(Type viewType, IPresentationAttribute attribute, CrossViewModelRequest request);

        public abstract ValueTask<bool> CloseActon(ICrossViewModel viewModel, IPresentationAttribute attribute);

    }
}
