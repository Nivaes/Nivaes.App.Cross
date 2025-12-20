namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using MvvmCross.ViewModels;

    [Obsolete]
    public interface ICrossInpcInterceptor
    {
        CrossInpcInterceptionResult Intercept(ICrossNotifyPropertyChanged sender, PropertyChangedEventArgs args);
        CrossInpcInterceptionResult Intercept(ICrossNotifyPropertyChanged sender, PropertyChangingEventArgs args);
    }
}