namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics;

    public class CrossCommand<T>
        : CrossCommandBase
    {
        #region Subclass
        /// <summary>Conver a action"/>.</summary>
        private struct DelegateExecute
        {
            /// <summary>Action to execute.</summary>
            private readonly Action<T> mExecute;

            /// <summary>Create a new instance of <see cref="DelegateExecute"/>.</summary>
            [DebuggerStepThrough]
            public DelegateExecute(Action<T> execute)
            {
                mExecute = execute;
            }

            /// <summary>Execute command.</summary>
            [DebuggerStepThrough]
            public void ExcuteCommand(T tag, object? parameter)
            {
                mExecute(tag);
            }
        }

        /// <summary>Conver a func.</summary>
        private struct DelegateCanExecute
        {
            /// <summary>Action to execute.</summary>
            private readonly Func<T, bool> mFunc;

            /// <summary>Create a new instance of <see cref="DelegateCanExecute"/>.</summary>
            [DebuggerStepThrough]
            public DelegateCanExecute(Func<T, bool> func)
            {
                mFunc = func;
            }

            /// <summary>Execute command.</summary>
            [DebuggerStepThrough]
            public bool ExcuteFunc(T tag, object? parameter)
            {
                return mFunc(tag);
            }
        }
        #endregion

        #region Properties
        /// <summary>Action to execute.</summary>
        private readonly Action<T, object?>? mExecute;

        /// <summary>Return <b>true</b> if can execute action, <b>false</b> in else case.</summary>
		private readonly Func<T, object?, bool>? mCanExecute;

        #region Tag
        /// <summary>The object that contains data about the command.</summary>
        private readonly T mTag;

        /// <summary>The object that contains data about the command.</summary>
        public T Tag
        {
            get { return mTag; }
        }
        #endregion
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="tag">The object that contains data about the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(T tag, Action<T> execute) : this(tag, execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="tag">The object that contains data about the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(T tag, Action<T, object?> execute) : this(tag, execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="tag">The object that contains data about the command.</param>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        [DebuggerStepThrough]
        public CrossCommand(T tag, Action<T> execute, Func<T, bool>? canExecute)
            : this(tag, execute != null ? new DelegateExecute(execute).ExcuteCommand : (Action<T, object?>?)null, canExecute != null ? new DelegateCanExecute(canExecute).ExcuteFunc : (Func<T, object?, bool>?)null)
        { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="tag">The object that contains data about the command.</param>
        /// <param name="execute">The object that contains data about the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        [DebuggerStepThrough]
        public CrossCommand(T tag, Action<T, object?>? execute, Func<T, object?, bool>? canExecute)
        {
            //if (execute == null) { throw new ArgumentNullException(nameof(execute)); }

            mExecute = execute;
            mCanExecute = canExecute;
            mTag = tag;
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
            return mCanExecute?.Invoke(this.mTag, parameter) ?? false;
        }

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        [DebuggerStepThrough]
        public override void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");

            mExecute?.Invoke(this.mTag, parameter);
        }
        #endregion
    }
}
