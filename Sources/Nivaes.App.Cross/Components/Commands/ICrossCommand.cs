namespace Nivaes.App.Cross
{
    using System.Windows.Input;

    public interface ICrossCommand
    : ICommand
    {
        void RaiseCanExecuteChanged();

        void Execute();

        bool CanExecute();
    }

    public interface ICrossCommand<in TParameter> : ICommand
    {
        void Execute(TParameter parameter);

        bool CanExecute(TParameter parameter);

        void RaiseCanExecuteChanged();
    }
}