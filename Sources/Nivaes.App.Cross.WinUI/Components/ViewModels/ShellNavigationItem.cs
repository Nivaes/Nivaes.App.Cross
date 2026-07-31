using Microsoft.UI.Xaml.Controls;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.WinUI
{
    public class ShellNavigationItem
    {
        #region Properties
        public string Label { get; private set; }

        public ICrossAsyncCommand? Command { get; private set; }

        public IconElement? Icon { get; private set; }

        public bool Reselectable { get; private set; }
        #endregion

        #region Constructors
        public ShellNavigationItem(string label, Symbol symbol, ICrossAsyncCommand command, bool reselectable = false)
            : this(label, command, reselectable)
        {
            Icon = new FontIcon { FontSize = 16, Glyph = ((char)symbol).ToString() };
        }

        public ShellNavigationItem(string label, IconElement? icon, ICrossAsyncCommand command, bool reselectable = false)
            : this(label, command, reselectable)
        {
            Icon = icon;
        }

        private ShellNavigationItem(string label, ICrossAsyncCommand command, bool reselectable)
        {
            Label = label;
            Command = command;
            Reselectable = reselectable;
        }
        #endregion
    }
}
