// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;

    public class MvxStrongCommandHelper
        : IMvxCommandHelper
    {
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged(object sender)
        {
            CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}
