namespace Nivaes.App.Cross
{
    public  interface IPressenterAction
    {
        public abstract ValueTask<bool> ShowActon(Type viewType, ICrossPresentationAttribute attribute, CrossViewModelRequest request);

        public abstract ValueTask<bool> CloseActon(ICrossViewModel viewModel, ICrossPresentationAttribute attribute);

    }
}
