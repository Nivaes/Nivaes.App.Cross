// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;

    public class MvxCommand<T>
        : MvxCommandBase
        , IMvxCommand, IMvxCommand<T>
    {
        private readonly Func<T, bool>? mCanExecute;
        private readonly Action<T> mExecute;

        public MvxCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action<T> execute, Func<T, bool>? canExecute)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => mCanExecute == null || mCanExecute((T)typeof(T).MakeSafeValueCore(parameter));

        public bool CanExecute()
            => CanExecute(null);

        public bool CanExecute(T parameter)
            => mCanExecute == null || mCanExecute(parameter);

        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;

            mExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute()
            => Execute(null);

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter)) return;

            mExecute(parameter);
        }
    }
}
