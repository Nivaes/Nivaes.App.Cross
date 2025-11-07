using System.Reflection;
using System.Windows.Input;

namespace Nivaes.App.Cross
{
    public class CrossCanExecuteChangedEventSubscription(
            ICommand source,
            EventHandler<EventArgs> eventHandler)
        : CrossWeakEventSubscription<ICommand, EventArgs>(source, CanExecuteChangedEventInfo, eventHandler)
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged")!;

        protected override Delegate CreateEventHandler() => new EventHandler(OnSourceEvent);
    }
}