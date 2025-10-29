namespace Nivaes.App.Cross.WinUI.Presenters
{
    using Nivaes.App.Cross.Presenters;

    public abstract class WinUIViewPresentation : CrossViewPresentation
    {
        protected AppDataModel WindowInformation { get; private set; }


        protected WinUIViewPresentation(AppDataModel windowInformation)
        {
            this.WindowInformation = windowInformation;
        }

        public override Task<bool> ShowView(Type viewType, ICrossViewModelRequest request)
        {
            var result = WindowInformation.MainFrame.Navigate(viewType, request.ViewModel);

            return Task.FromResult(result);
        }

        public override Task<bool> CloseView(ICrossViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
