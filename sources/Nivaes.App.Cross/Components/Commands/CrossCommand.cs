namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics;

    public abstract class CrossCommand
        : CrossCommandBase
    {
        #region Subclass
        /// <summary>Conver a action.</summary>
        private struct DelegateExecute
        {
            /// <summary>Action to execute.</summary>
            private readonly Action mExecute;

            /// <summary>Create a new instance of <see cref="DelegateExecute"/>.</summary>
            [DebuggerStepThrough]
            public DelegateExecute(Action execute)
            {
                mExecute = execute;
            }

            /// <summary>Execute command.</summary>
            [DebuggerStepThrough]
            public void ExcuteCommand(object? o)
            {
                mExecute();
            }
        }

        /// <summary>Conver a func.</summary>
        private struct DelegateCanExecute
        {
            /// <summary>Action to execute.</summary>
            private readonly Func<bool>? mFunc;

            /// <summary>Create a new instance of <see cref="DelegateExecute"/>.</summary>
            [DebuggerStepThrough]
            public DelegateCanExecute(Func<bool> func)
            {
                mFunc = func;
            }

            /// <summary>Execute command.</summary>
            [DebuggerStepThrough]
            public bool ExcuteFunc(object? o)
            {
                return mFunc != null ? mFunc() : false;
            }
        }
        #endregion

        #region Properties
        /// <summary>Action to execute.</summary>
        private readonly Action<object?>? mExecute;
        /// <summary>Return <b>true</b> if can execute action, <b>false</b> in else case.</summary>
        private readonly Func<object?, bool>? mCanExecute;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(Action execute) : this(execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(Action<object?> execute) : this(execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(Action execute, Func<bool>? canExecute)
            : this(execute != null ? new DelegateExecute(execute).ExcuteCommand : (Action<object?>?)null, canExecute != null ? new DelegateCanExecute(canExecute).ExcuteFunc : (Func<object?, bool>?)null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        [DebuggerStepThrough]
        public CrossCommand(Action<object?>? execute, Func<object?, bool>? canExecute)
        {
            //if (execute == null) { throw new ArgumentNullException(nameof(execute)); }

            mExecute = execute;
            mCanExecute = canExecute;
        }
        #endregion

        #region ICommand
        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// <b>True</b> if this command can be executed; otherwise, <b>false</b>.
        /// </returns>
        [DebuggerStepThrough]
        public override bool CanExecute(object? parameter)
        {
            return mCanExecute?.Invoke(parameter) ?? false;
        }

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        [DebuggerStepThrough]
        public override void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");

            mExecute?.Invoke(parameter);
        }
        #endregion
    }
}
