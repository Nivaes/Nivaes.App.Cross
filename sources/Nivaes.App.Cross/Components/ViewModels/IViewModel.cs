namespace Nivaes.App.Cross
{
    using System.ComponentModel;

    public interface IViewModel
        : INotifyPropertyChanged
    {
        void ViewCreated();
    }

    public interface IViewModel<TParameter>
        : IViewModel
    {
        void Prepare(TParameter parameter);
    }
}
