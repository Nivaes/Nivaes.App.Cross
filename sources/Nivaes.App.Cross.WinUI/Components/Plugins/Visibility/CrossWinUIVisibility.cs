using WinUiVisibility = Microsoft.UI.Xaml.Visibility;

namespace Nivaes.App.Cross.WinUI
{
    public class CrossWinUIVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            return visibility == CrossVisibility.Visible
                       ? WinUiVisibility.Visible
                       : WinUiVisibility.Collapsed;
        }
    }
}
