using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public interface ICrossNotifyPropertyChanged : 
        INotifyPropertyChanged, 
        INotifyPropertyChanging
    {
        // this ShouldAlwaysRaiseInpcOnUserInterfaceThread is not a Property so as to avoid Inpc pollution
        bool ShouldAlwaysRaiseInpcOnUserInterfaceThread();

        void ShouldAlwaysRaiseInpcOnUserInterfaceThread(bool value);

        bool ShouldRaisePropertyChanging();

        void ShouldRaisePropertyChanging(bool value);

#pragma warning disable CA1030 // Use events where appropriate
        bool RaisePropertyChanging<T>(T newValue, Expression<Func<T>> propertyExpression);

        bool RaisePropertyChanging<T>(T newValue, string whichProperty = "");

        bool RaisePropertyChanging<T>(CrossPropertyChangingEventArgs<T> changingArgs);

        Task RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression);

        Task RaisePropertyChanged(string whichProperty = "");

        Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
#pragma warning restore CA1030 // Use events where appropriate
    }
}
