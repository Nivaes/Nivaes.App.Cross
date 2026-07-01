namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Reflection;

    public class CrossNotifyPropertyChangedEventSubscription(
            INotifyPropertyChanged source,
            EventHandler<PropertyChangedEventArgs> targetEventHandler)
        : CrossWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>(
            source, PropertyChangedEventInfo, targetEventHandler)
    {
        private static readonly EventInfo PropertyChangedEventInfo =
            typeof(INotifyPropertyChanged).GetEvent("PropertyChanged")!;

        // This code ensures the PropertyChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
        {
            iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
        }

        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(OnSourceEvent);
        }
    }
}