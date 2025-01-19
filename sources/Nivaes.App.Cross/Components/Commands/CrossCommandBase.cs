namespace Nivaes.App.Cross
{
    public abstract class CrossCommandBase
        : ICrossCommand
    {
        #region CanExecuteChanged
        private readonly List<WeakReference> canExecuteChangedEventListeners = [];

        /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
        public event EventHandler? CanExecuteChanged
        {
            add 
            {
                canExecuteChangedEventListeners.Add(new WeakReference(value));
            }
            remove
            {
                canExecuteChangedEventListeners.RemoveAll(wr =>
                {
                    var target = wr.Target as EventHandler;
                    if (target == null)
                        return false;
                    else
                        return target == value;
                });
            }
        }
        #endregion

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }
}
