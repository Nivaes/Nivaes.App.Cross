namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using MvvmCross.ViewModels;

    public interface ICrossInpcInterceptor
    {
        MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args);
        MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangingEventArgs args);
    }
}
