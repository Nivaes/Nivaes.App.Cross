using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.WinUI
{
    public interface IShellModel
    {
        IEnumerable<ShellNavigationItem> PrimaryItems { get; }

        IEnumerable<ShellNavigationItem> SecondaryItems { get; }

        ICrossAsyncCommand ShowSettingsCommand { get; }

        ICrossAsyncCommand ShowAccountCommand { get; }
    }
}
