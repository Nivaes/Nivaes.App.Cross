// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System.Windows.Input;

    public interface IMvxCommand
        : ICommand
    {
        void RaiseCanExecuteChanged();

        void Execute();

        bool CanExecute();
    }

    public interface IMvxCommand<T>
        : ICommand
    {
        void RaiseCanExecuteChanged();

        void Execute(T parameter);

        bool CanExecute(T parameter);
    }
}
