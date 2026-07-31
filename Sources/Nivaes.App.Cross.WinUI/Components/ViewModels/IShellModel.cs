using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.WinUI
{
    public interface IShellModel
    {
        IEnumerable<IEnumerable<ShellNavigationItem>> Items { get; }

        ICrossAsyncCommand ShowSettingsCommand { get; }

        ICrossAsyncCommand ShowAccountCommand { get; }
    }
}
