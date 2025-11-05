namespace Nivaes.App.Cross.Droid.Presenters
{
    using Nivaes.App.Cross.Presenters;

    public sealed class CrossActivityViewPresentation : 
        CrossDroidViewPresentation
    {
        public CrossActivityViewPresentation(AppDataModel appDataModel)
            : base(appDataModel)
        {

        }

        //private Task<bool> ShowPage(/*IWindowsFrame rootFrame,*/ Type viewType)
        //{
        //    //_frame.Navigate(viewType);
        //}

        //private Task<bool> ClosePage(/*IWindowsFrame rootFrame,*/ Type viewType)
        //{
        //    //_frame.GoBack();
        //}
    }
}
