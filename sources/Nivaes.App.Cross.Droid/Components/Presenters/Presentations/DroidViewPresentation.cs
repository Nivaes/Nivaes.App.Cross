namespace Nivaes.App.Cross.Droid.Presenters
{
    using Nivaes.App.Cross.Presenters;

    public abstract class DroidViewPresentation : ViewPresentation
    {
        //protected WindowInformation WindowInformation { get; private set; }

        //private readonly object _windowInformationLock = new();

        protected DroidViewPresentation(/*WindowInformation windowInformation*/)
        {
            //this.WindowInformation = windowInformation;
        }

        public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        {
            //var aa = WindowInformation.MainFrame.UnderlyingControl.DispatcherQueue.HasThreadAccess;

            //var result = WindowInformation.MainFrame.Navigate(viewType, new object());

            ////var result = WindowInformation.MainFrame.Navigate(typeof(Root2View), new object());

            return Task.FromResult(false);
        }

        public override Task<bool> CloseView(IViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
