// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;

    public class MvxCommand
        : MvxCommandBase
        , IMvxCommand
    {
        private readonly Func<bool>? mCanExecute;
        private readonly Action mExecute;

        public MvxCommand(Action execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action execute, Func<bool>? canExecute)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => mCanExecute == null || mCanExecute();

        public bool CanExecute()
            => CanExecute(null);

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                mExecute();
            }
        }

        public void Execute()
            => Execute(null);
    }
}
