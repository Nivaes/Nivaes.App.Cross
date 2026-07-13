namespace Nivaes.App.Cross
{
    public  interface IPressenterAction
    {
        public abstract Task<bool> ShowActon(Type view, ICrossPresentationAttribute attribute, CrossViewModelRequest request);

        public abstract Task<bool> CloseActon(ICrossViewModel viewModel, ICrossPresentationAttribute attribute);

    }
}
