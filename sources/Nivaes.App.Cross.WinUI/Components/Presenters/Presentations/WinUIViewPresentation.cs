namespace Nivaes.App.Cross.WinUI.Presenters
{
    using Nivaes.App.Cross.Presenters;

    public abstract class WinUIViewPresentation : ViewPresentation
    {
        protected AppDataModel WindowInformation { get; private set; }

        //private readonly object _windowInformationLock = new();

        protected WinUIViewPresentation(AppDataModel windowInformation)
        {
            this.WindowInformation = windowInformation;
        }

        public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        {
            var result = WindowInformation.MainFrame.Navigate(viewType, new object());

            return Task.FromResult(result);
        }

        public override Task<bool> CloseView(IViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
